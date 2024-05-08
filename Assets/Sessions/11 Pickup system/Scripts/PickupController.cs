using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.PlayerLoop;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PickupController : MonoBehaviour//, IPicker
{
    public enum SphereQuarter
    {
        UpperRight,
        UpperLeft,
        LowerLeft,
        LowerRight
    }

    public struct SphereQuarterData
    {
        public Vector3 localDirection;
        public Vector2 animationDirection;
        public SphereQuarter quarterId;

        public SphereQuarterData(Vector3 localDirection, SphereQuarter quarterId, Vector2 animationDirection)
        {
            this.localDirection = localDirection;
            this.quarterId = quarterId;
            this.animationDirection = animationDirection;
        }
    }
    
    [SerializeField] private float maxNonTurnAngle;
    [SerializeField] private float maxReachingDistance;
    [SerializeField] private float maxPerSideAngle;
    [SerializeField] private Transform torsoReference;
    [SerializeField] private TwoBoneIKConstraint rightHand;
    [SerializeField] private TwoBoneIKConstraint leftHand;
    [SerializeField] private Animator animator;

    public List<GameObject> availablePickables = new List<GameObject>();
    
    

    private List<SphereQuarterData> sphereQuarterDirections;
    private int currentNearestQuarterDirection = -1;
    private TwoBoneIKConstraint currentEditedConstraint = null;
    private GameObject currentGrabbedItem;
    private bool grabbedItemIsSnapping;

    public Vector3 GetQuarterSurfaceVector(SphereQuarter quarter)
    {
        Vector3 ret = Vector3.zero;
        switch (quarter)
        {
            case SphereQuarter.UpperRight:
                return Quaternion.Euler(-maxPerSideAngle * 0.5f, maxPerSideAngle * 0.5f, 0) * Vector3.forward;
            case SphereQuarter.LowerLeft:
                return Quaternion.Euler(maxPerSideAngle * 0.5f, -maxPerSideAngle * 0.5f, 0) * Vector3.forward;
            case SphereQuarter.UpperLeft:
                return Quaternion.Euler(-maxPerSideAngle * 0.5f, -maxPerSideAngle * 0.5f, 0) * Vector3.forward;
            case SphereQuarter.LowerRight:
                return Quaternion.Euler(maxPerSideAngle * 0.5f, maxPerSideAngle * 0.5f, 0) * Vector3.forward;
        }

        return ret;
    }

    private void UpdateQuarterDirections()
    {
        sphereQuarterDirections = new List<SphereQuarterData>()
        {
            new SphereQuarterData(GetQuarterSurfaceVector(SphereQuarter.UpperRight), SphereQuarter.UpperRight,new Vector2(1,1)),
            new SphereQuarterData(GetQuarterSurfaceVector(SphereQuarter.UpperLeft), SphereQuarter.UpperLeft, new Vector2(-1,1)),
            new SphereQuarterData(GetQuarterSurfaceVector(SphereQuarter.LowerLeft), SphereQuarter.LowerLeft, new Vector2(-1,-1)),
            new SphereQuarterData(GetQuarterSurfaceVector(SphereQuarter.LowerRight), SphereQuarter.LowerRight, new Vector2(1,-1)),
        };
    }

    public int GetNearestQuarterDirection(Vector3 localPosition)
    {
        int ret = 0;
        for (int i = 0; i < sphereQuarterDirections.Count; i++)
        {
            SphereQuarterData currentNearest = sphereQuarterDirections[ret];
            float dot1 = Vector3.Dot(currentNearest.localDirection, localPosition.normalized);
            float dot2 = Vector3.Dot(sphereQuarterDirections[i].localDirection, localPosition.normalized);
            if (dot2 > dot1)
            {
                ret = i;
            }
        }

        return ret;
    }

    public void PickUpNearest()
    {
        //Find nearest non-null pickable
        currentGrabbedItem = availablePickables.Where(pickable => pickable != null).OrderBy(pickable =>
        {
            Vector3 itemDir = torsoReference.position - pickable.transform.position;
            return Vector3.SqrMagnitude(itemDir) * (1-Mathf.Abs(Vector3.Dot(itemDir.normalized, torsoReference.forward)));
        }).FirstOrDefault();

        if (currentGrabbedItem == default) return;
        Vector3 localSpaceItemDir = torsoReference.InverseTransformPoint(currentGrabbedItem.transform.position);
        currentNearestQuarterDirection = GetNearestQuarterDirection(localSpaceItemDir);

        Vector2 animDir = sphereQuarterDirections[currentNearestQuarterDirection].animationDirection;

        currentEditedConstraint = animDir.x < 0 ? leftHand : rightHand;
        currentEditedConstraint.data.target.position = TorsoReference.TransformPoint(localSpaceItemDir);

        animator.SetFloat("PickupDirectionX", animDir.x);
        animator.SetFloat("PickupDirectionY", animDir.y);
        animator.SetTrigger("Pickup");
        
    }

    private void OnValidate()
    {
        UpdateQuarterDirections();
    }

    private void Update()
    {
        float constraintModulator = animator.GetFloat("IKPickupWeight");

        if (currentEditedConstraint != null)
        {
            currentEditedConstraint.weight = constraintModulator;
        }

        if (currentGrabbedItem != null && currentGrabbedItem.transform.parent != null)
        {
            currentGrabbedItem.transform.localPosition = Vector3.Lerp(currentGrabbedItem.transform.localPosition,
                Vector3.zero, Time.deltaTime * 5);
        }
    }

    public void OnGrab()
    {
        return;
        currentGrabbedItem.transform.parent = currentEditedConstraint.transform;
    }


    public float MaxNonTurnAngle => maxNonTurnAngle;

    public float MaxReachingDistance => maxReachingDistance;

    public float MaxPerSideAngle => maxPerSideAngle;

    public Transform TorsoReference => torsoReference ? torsoReference : transform;
    public List<SphereQuarterData> SphereQuarterDirections => sphereQuarterDirections;

    public int CurrentNearestQuarterDirection => currentNearestQuarterDirection;
}
