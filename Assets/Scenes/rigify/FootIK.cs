using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIK : MonoBehaviour
{
    [SerializeField] private Transform detectionReference;
    [SerializeField] private Transform foot;
    [SerializeField][Range(0,1)] private float detectionRange;
    [SerializeField] private float maxDetectionDistance;

    private bool hasTarget;
    private RaycastHit ikTarget;
    // Start is called before the first frame update

    public Vector3 GetDetectionStartPosition()
    {
        Vector3 referenceSpacePosition = detectionReference.InverseTransformPoint(foot.position);
        Vector3 ret = new Vector3(referenceSpacePosition.x, referenceSpacePosition.y * detectionRange, referenceSpacePosition.z);
        return detectionReference.TransformPoint(ret);
    }

    private void GetTargetPosition()
    {
        hasTarget = Physics.Raycast(GetDetectionStartPosition(), -detectionReference.up, out ikTarget, maxDetectionDistance);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        GetTargetPosition();
    }

    public Transform DetectionReference => detectionReference;
    public float MaxDetectionDistance => maxDetectionDistance;
    public bool HasTarget => hasTarget;
}
