using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

[Serializable]

public class FloatEvent : UnityEvent<float>
{

}

[RequireComponent(typeof(Animator))]  
public class FootIK : MonoBehaviour
{
    [SerializeField] private Transform detectionReference;
    [SerializeField] private Transform foot;
    [SerializeField][Range(0,1)] private float detectionRange;
    [SerializeField] private float maxDetectionDistance;
    [SerializeField] private AvatarIKGoal ikGoal;
    [SerializeField] private Vector2 snapOffsets;
    [SerializeField] private string snapOffsetParameter;
    [SerializeField] private float snapSpeed = 15;
    [SerializeField] private Vector3 snapRotationOffset;
    [SerializeField] private Transform root;

    public FloatEvent onIkSolved;
    Animator animator;

    private bool hasTarget;
    private RaycastHit ikTarget;
    private Vector3 currentIkPosition;
    // Start is called before the first frame update


    
    public Vector3 GetDetectionStartPosition()
    {
        Vector3 referenceSpacePosition = detectionReference.InverseTransformPoint(foot.position);
        Vector3 ret = new Vector3(referenceSpacePosition.x, referenceSpacePosition.y * detectionRange, referenceSpacePosition.z);
        return detectionReference.TransformPoint(ret);
    }

    private bool GetTargetPosition()
    {
        return Physics.Raycast(GetDetectionStartPosition(), -detectionReference.up, out ikTarget, maxDetectionDistance);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        hasTarget = GetTargetPosition();
        currentIkPosition = Vector3.Lerp(currentIkPosition, ikTarget.point, Time.deltaTime * snapSpeed);
        animator.SetIKPositionWeight(ikGoal, 1.0f);
        float snapInterpolator = animator.GetFloat(snapOffsetParameter);
        float solvedSnapOffset = Mathf.Lerp(snapOffsets.x, snapOffsets.y, snapInterpolator);    
        animator.SetIKPosition(ikGoal, currentIkPosition + detectionReference.up * snapOffsets.y);

        animator.SetIKRotationWeight(ikGoal, snapInterpolator);
        Quaternion rot = Quaternion.LookRotation(ikTarget.normal) * Quaternion.Euler(snapRotationOffset);
        animator.SetIKRotation(ikGoal, rot);

        Vector3 characterSpaceFoot = root.InverseTransformPoint(ikTarget.point);

        onIkSolved?.Invoke(characterSpaceFoot.y);
    }

    public Transform DetectionReference => detectionReference;
    public float MaxDetectionDistance => maxDetectionDistance;
    public bool HasTarget => hasTarget;
}
