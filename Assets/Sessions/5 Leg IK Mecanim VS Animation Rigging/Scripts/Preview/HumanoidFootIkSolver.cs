using System;
using System.Linq;
using UnityEngine;

public class HumanoidFootIkSolver : MonoBehaviour
{
    public struct SnapTargetData
    {
        public RaycastHit hit;
        public float offsetThreshold; //0 = no offset, 1 = offset

        public SnapTargetData(RaycastHit hit, float offsetThreshold)
        {
            this.hit = hit;
            this.offsetThreshold = offsetThreshold;
        }
    }
    
    // [SerializeField] private AvatarIKGoal ikGoal;
    // [SerializeField] private AvatarIKHint ikHint;
    [SerializeField] private Transform detectionReference;
    [SerializeField] private Transform rightFoot;
    [SerializeField] private Transform leftFoot;
    [SerializeField][Range(0,1)] private float detectionStart;
    [SerializeField] private float maxDetectionDistance;
    [SerializeField] private float surfaceOffset;
    [SerializeField] private Vector3 rotationOffset;

    [SerializeField] private float hipsOffset;
    [SerializeField] private Transform hipBone;
    [SerializeField] private string detectionTag;

    private bool hasLeftSnapTarget;
    private bool hasRightSnapTarget;
    private RaycastHit leftSnapTarget;
    private RaycastHit rightSnapTarget;
    private Vector3 leftSmoothSnapPosition;
    private Vector3 leftSmoothSnapNormal;
    
    private Vector3 rightSmoothSnapPosition;
    private Vector3 rightSmoothSnapNormal;
    private Animator anim;

    private Vector3 targetTransformPos;
    private Vector3 smoothTransformPos;

    public Transform GetFoot(AvatarIKGoal goal)
    {
        switch (goal)
        {
            case AvatarIKGoal.LeftFoot:
                return leftFoot;
            case AvatarIKGoal.RightFoot:
                return rightFoot;
            default:
                return null;
        }
    }

    public bool QuerySnapForFoot(AvatarIKGoal goal)
    {
        switch (goal)
        {
            case AvatarIKGoal.LeftFoot:
                return hasLeftSnapTarget;
            case AvatarIKGoal.RightFoot:
                return hasRightSnapTarget;
            default:
                return false;
        }
    }
    private void SetSnapForFoot(AvatarIKGoal goal, bool value)
    {
        switch (goal)
        {
            case AvatarIKGoal.LeftFoot:
                hasLeftSnapTarget = value;
                break;
            case AvatarIKGoal.RightFoot:
                hasRightSnapTarget = value;
                break;
            default:
                break;
        }
    }

    public RaycastHit GetSnapData(AvatarIKGoal goal)
    {
        switch (goal)
        {
            case AvatarIKGoal.LeftFoot:
                return leftSnapTarget;
            case AvatarIKGoal.RightFoot:
                return rightSnapTarget;
            default:
                throw new ArgumentException($"Avatar IK Goal ({goal}) is not valid for IK snap");
        }
    }

    private void SetSnapData(AvatarIKGoal goal, RaycastHit value)
    {
        switch (goal)
        {
            case AvatarIKGoal.LeftFoot:
                leftSnapTarget = value;
                break;
            case AvatarIKGoal.RightFoot:
                rightSnapTarget = value;
                break;
            default:
                break;
        }
    }

    public Vector3 GetDetectionStartPoint(AvatarIKGoal goal)
    {
        if (goal != AvatarIKGoal.LeftFoot && goal != AvatarIKGoal.RightFoot)
        {
            throw new ArgumentException($"Avatar IK Goal ({goal}) is not valid for IK snap");
        }
        Vector3 referenceSpacePosition = detectionReference.InverseTransformPoint(GetFoot(goal).position);
        Vector3 startPoint = new Vector3(referenceSpacePosition.x, Mathf.Lerp(0, referenceSpacePosition.y, detectionStart), referenceSpacePosition.z);
        return detectionReference.TransformPoint(startPoint);
    }

    private Vector3 GetSmoothSnapPosition(AvatarIKGoal goal)
    {
        switch (goal)
        {
            case AvatarIKGoal.LeftFoot:
                return leftSmoothSnapPosition;
            case AvatarIKGoal.RightFoot:
                return rightSmoothSnapPosition;
            default:
                throw new ArgumentException($"Avatar IK Goal ({goal}) is not valid for IK snap");
        }
    }
    
    private void SetSmoothSnapPosition(AvatarIKGoal goal, Vector3 value)
    {
        switch (goal)
        {
            case AvatarIKGoal.LeftFoot:
                leftSmoothSnapPosition = value;
                break;
            case AvatarIKGoal.RightFoot:
                rightSmoothSnapPosition = value;
                break;
            default:
                throw new ArgumentException($"Avatar IK Goal ({goal}) is not valid for IK snap");
        }
    }
    
    private Vector3 GetSmoothSnapNormal(AvatarIKGoal goal)
    {
        switch (goal)
        {
            case AvatarIKGoal.LeftFoot:
                return leftSmoothSnapNormal;
            case AvatarIKGoal.RightFoot:
                return rightSmoothSnapNormal;
            default:
                throw new ArgumentException($"Avatar IK Goal ({goal}) is not valid for IK snap");
        }
    }
    
    private void SetSmoothSnapNormal(AvatarIKGoal goal, Vector3 value)
    {
        switch (goal)
        {
            case AvatarIKGoal.LeftFoot:
                leftSmoothSnapNormal = value;
                break;
            case AvatarIKGoal.RightFoot:
                rightSmoothSnapNormal = value;
                break;
            default:
                throw new ArgumentException($"Avatar IK Goal ({goal}) is not valid for IK snap");
        }
    }

    private RaycastHit[] GetSuitableSurfaces(AvatarIKGoal goal)
    {
        return Physics.RaycastAll(GetDetectionStartPoint(goal), -detectionReference.up,maxDetectionDistance);
    }

    private bool GetNearestSurfacePoint(AvatarIKGoal goal, RaycastHit[] hits, out RaycastHit point)
    {
        try
        {
            Vector3 detectionStartPoint = GetDetectionStartPoint(goal);
            RaycastHit hit = hits.Where(h => h.collider.gameObject.CompareTag(detectionTag)).OrderBy(hit => Vector3.Distance(hit.point, detectionReference.position)).First();
            point = hit;
            return true;
        }
        catch
        {
            point = new RaycastHit
            {
                point = GetFoot(goal).position,
                normal = detectionReference.up
            };
            return false;
        }
    }

    private void ApplyIkAdjustmentsToFoot(AvatarIKGoal goal)
    {
        bool hasTarget = GetNearestSurfacePoint(goal, GetSuitableSurfaces(goal), out RaycastHit data);
        SetSnapForFoot(goal,hasTarget);
        SetSnapData(goal, data);
        if (!hasTarget) return;
        SetSmoothSnapPosition(goal,Vector3.Lerp(GetSmoothSnapPosition(goal), data.point, Time.deltaTime * 30));
        SetSmoothSnapNormal(goal,Vector3.Slerp(GetSmoothSnapPosition(goal), data.normal, Time.deltaTime * 30));
        anim.SetIKPositionWeight(goal,1);
        //anim.SetIKRotationWeight(goal,1);
        anim.SetIKPosition(goal, GetSmoothSnapPosition(goal) + detectionReference.up * surfaceOffset);
        Quaternion targetRotation = Quaternion.LookRotation(GetSmoothSnapPosition(goal)) * Quaternion.Euler(rotationOffset);
        //anim.SetIKRotation(goal, targetRotation);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    private void OnAnimatorMove()
    {
        transform.position = smoothTransformPos;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        targetTransformPos = transform.position;

        ApplyIkAdjustmentsToFoot(AvatarIKGoal.LeftFoot);
        ApplyIkAdjustmentsToFoot(AvatarIKGoal.RightFoot);
        float leftFootHeightOffset = transform.InverseTransformPoint(GetSnapData(AvatarIKGoal.LeftFoot).point).y;
        float rightFootHeightOffset = transform.InverseTransformPoint(GetSnapData(AvatarIKGoal.RightFoot).point).y;
        if (leftFootHeightOffset > 0.1f && rightFootHeightOffset > 0.1f)
        {
            targetTransformPos =
                transform.TransformPoint(new Vector3(0, Mathf.Min(leftFootHeightOffset, rightFootHeightOffset), 0));
        }

        smoothTransformPos = Vector3.Lerp(smoothTransformPos, targetTransformPos, Time.deltaTime * 5);
    }

    public Transform DetectionReference => detectionReference;
    public float MaxDetectionDistance => maxDetectionDistance;
    public float SurfaceOffset => surfaceOffset;
}
