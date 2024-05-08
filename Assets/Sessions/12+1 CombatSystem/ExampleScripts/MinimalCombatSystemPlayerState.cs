using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimalCombatSystemPlayerState : MonoBehaviour
{
    [SerializeField] private float maxBaseStamina;

    private float currentStamina;

    public FloatEvent onStaminaModified;

    public void ModifyStamina(float modifyValue)
    {
        currentStamina += modifyValue;
        onStaminaModified?.Invoke(currentStamina / maxBaseStamina);
    }
    
    private void Awake()
    {
        ModifyStamina(maxBaseStamina);
    }

    public float CurrentStamina => currentStamina;
}
