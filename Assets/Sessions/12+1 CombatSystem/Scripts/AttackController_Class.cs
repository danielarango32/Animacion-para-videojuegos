using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class AttackController_Class : MonoBehaviour
{

    [SerializeField] private float chargeSpeed = 0.2f;
    private Animator animator;
    Animator Animator
    {
        get
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            return animator;
        }
    }
    
    public void OnLightAttack(InputAction.CallbackContext ctx)
    {
        bool val = ctx.ReadValueAsButton();
        if (val)
        {
            //Attack
            Animator.SetTrigger("Attack");
            Animator.SetBool("HeavyAttack", false);
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext ctx)
    {
        bool val = ctx.ReadValueAsButton();
        if (val)
        {
            Animator.SetTrigger("Attack");
            Animator.SetBool("HeavyAttack", true);
            Animator.SetFloat("ChargeSpeed", chargeSpeed);
        }
        else
        {
            Animator.SetFloat("ChargeSpeed", 1);
        }
    }
}
