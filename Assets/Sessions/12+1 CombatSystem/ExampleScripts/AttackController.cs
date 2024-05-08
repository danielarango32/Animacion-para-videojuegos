using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class AttackController : MonoBehaviour
{
    [SerializeField] private float lightAttackStaminaConsumption = 10;
    [SerializeField] private float heavyAttackStaminaConsumption = 30;
    [SerializeField] private float staminaRegenRate = 15;

    private MinimalCombatSystemPlayerState playerState;
    
    [SerializeField][Range(0,1)]
    private float minChargeSpeed;

    private Animator anim;
    private Animator Animator
    {
        get
        {
            if (anim == null)
            {
                anim = GetComponent<Animator>();
            }

            return anim;
        }
    }

    private MinimalCombatSystemPlayerState PlayerState
    {
        get
        {
            if (playerState == null)
            {
                playerState = GetComponent<MinimalCombatSystemPlayerState>();
            }

            return playerState;
        }
    }

    private bool canAttackAgain;

    private void Update()
    {
        canAttackAgain = Animator.GetFloat("ComboFollowUpWindow") > 0.7f;
        if (canAttackAgain)
        {
            PlayerState.ModifyStamina(staminaRegenRate * Time.deltaTime);
        }
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (!canAttackAgain) return;
        if (PlayerState.CurrentStamina <= 0) return;
        PlayerState.ModifyStamina(-lightAttackStaminaConsumption);
        bool val = ctx.ReadValueAsButton();
        if (!val) return;
        Animator.SetBool("StrongAttack", false);
        Animator.SetTrigger("Attack");
    }

    public void OnHeavyAttack(InputAction.CallbackContext ctx)
    {
        
        
        
        bool val = ctx.ReadValueAsButton();
        if (val)
        {
            if (!canAttackAgain) return;
            if (PlayerState.CurrentStamina <= 0) return;
            PlayerState.ModifyStamina(-heavyAttackStaminaConsumption);
            Animator.SetFloat("ChargeSpeed", minChargeSpeed);
            Animator.SetBool("StrongAttack", true);
            Animator.SetTrigger("Attack");
        }
        else
        {
            Animator.SetFloat("ChargeSpeed", 1);
        }
    }
}
