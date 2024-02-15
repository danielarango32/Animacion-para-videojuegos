using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(AimConstraint))]
public class AimConstraintAnim : MonoBehaviour
{
    [SerializeField] private Animator animator;
    AimConstraint aimConstraint;
    int aimWeightId;
    private void Awake()
    {
        aimConstraint = GetComponent<AimConstraint>();
        aimWeightId = Animator.StringToHash("AimWeight");
    }

    private void LateUpdate()
    {
        aimConstraint.weight = animator.GetFloat(aimWeightId);
    }
}
