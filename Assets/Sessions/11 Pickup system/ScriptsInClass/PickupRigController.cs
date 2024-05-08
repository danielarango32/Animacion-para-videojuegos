using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PickupRigController : MonoBehaviour
{
    public struct QuadrantData
    {
        public Vector3 localDirection;
        public Vector2 animationDirection;
        public int handIndex;
    }
    
    [SerializeField] private float maxReachingDistance;
    [SerializeField] private float perQuadrantAngle;
    [SerializeField] private Transform targetReference;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform[] ikHands;
    [SerializeField] private TwoBoneIKConstraint[] hands;

    public List<Transform> availableItems = new List<Transform>();
    
    private List<QuadrantData> quadrants = new List<QuadrantData>();

    private int ikHandIndex;

    private Transform grabbedObject;
    
    private void UpdateQuadrantData()
    {
        quadrants = new List<QuadrantData>()
        {
            new QuadrantData
            {
                //UpperLeft
                localDirection = Quaternion.Euler(-perQuadrantAngle * 0.5f, -perQuadrantAngle * 0.5f, 0) * Vector3.forward * maxReachingDistance,
                animationDirection = new Vector2( -1,1),
                handIndex = 0
            },
            new QuadrantData
            {
                //UpperRight
                localDirection = Quaternion.Euler(-perQuadrantAngle * 0.5f, perQuadrantAngle * 0.5f, 0) * Vector3.forward * maxReachingDistance,
                animationDirection = new Vector2( 1,1),
                handIndex = 1
            },
            new QuadrantData
            {
                //LowerRight
                localDirection = Quaternion.Euler(perQuadrantAngle * 0.5f, perQuadrantAngle * 0.5f, 0) * Vector3.forward * maxReachingDistance,
                animationDirection = new Vector2( 1,-1),
                handIndex = 1
            },
            new QuadrantData
            {
                //LowerLeft
                localDirection = Quaternion.Euler(perQuadrantAngle * 0.5f, -perQuadrantAngle * 0.5f, 0) * Vector3.forward * maxReachingDistance,
                animationDirection = new Vector2( -1,-1),
                handIndex = 0
            }
        };
    }

    private void SetUpIkConstraint(int id, Vector3 pickUpPosition)
    {
        ikHands[id].position = pickUpPosition;
        ikHandIndex = id;
    }
    
    public void PickUpNearestObject()
    {
        grabbedObject = availableItems.OrderBy(item =>
        {
            Vector3 itemDir = targetReference.position - item.position;
            float sqrMagnitude = Vector3.SqrMagnitude(itemDir);
            float dot = Mathf.Abs(Vector3.Dot(itemDir.normalized, TargetReference.forward));
            return sqrMagnitude * dot;
        }).FirstOrDefault();

        if (grabbedObject == default) return;

        Vector3 localItemPosition = targetReference.InverseTransformPoint(grabbedObject.position);
        int nearestQuadrantId = 0;
        Vector3 normalizedLocalItemPosition = localItemPosition.normalized;
        for (int i = 0; i < quadrants.Count; i++)
        {
            Vector3 currentNearest = quadrants[nearestQuadrantId].localDirection;
             
            float dot = Vector3.Dot(currentNearest, normalizedLocalItemPosition);
            if (Vector3.Dot(normalizedLocalItemPosition, quadrants[i].localDirection) > dot)
            {
                nearestQuadrantId = i;
            }
        }

        QuadrantData quadrant = quadrants[nearestQuadrantId];
        anim.SetFloat("PickupX", quadrant.animationDirection.x);
        anim.SetFloat("PickupY", quadrant.animationDirection.y);
        anim.SetTrigger("PickUp");
        
        SetUpIkConstraint(quadrant.handIndex, grabbedObject.position);
    }

    public void OnGrab()
    {
        if (grabbedObject == null) return;
        grabbedObject.parent = hands[ikHandIndex].transform;
    }

    public void OnStashed()
    {
        if (grabbedObject == null) return;
        grabbedObject.gameObject.SetActive(false);
    }
    
    private void OnValidate()
    {
        UpdateQuadrantData();
    }

    private void Update()
    {
        hands[ikHandIndex].weight = anim.GetFloat("IKPickupWeight2");
        if (grabbedObject != null && grabbedObject.parent != null)
        {
            grabbedObject.localPosition =
                Vector3.Lerp(grabbedObject.localPosition, Vector3.zero, Time.deltaTime * 10.0f);
        }
    }


    public Transform TargetReference => targetReference == null ? transform : targetReference;
    public float MaxReachingDistance => maxReachingDistance;

    public List<QuadrantData> Quadrants => quadrants;
}
