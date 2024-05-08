using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(AimConstraint))]
public class AimConstraintAnimationApplier : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private AimConstraint constraint;

    private int aimWeightId;
    private void Awake()
    {
        constraint = GetComponent<AimConstraint>();
        aimWeightId = Animator.StringToHash("AimWeight");
    }

    private void LateUpdate()
    {
       // float value = animator.GetFloat("a");
        constraint.weight = animator.GetFloat(aimWeightId);
    }
}
