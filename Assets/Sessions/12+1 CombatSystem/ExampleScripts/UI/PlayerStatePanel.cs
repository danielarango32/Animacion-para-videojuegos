using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatePanel : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider staminaSlider;

    private float targetStaminaValue;
    private float currentStaminaValue;
    private float staminaSmoothVelocity;
    
    public void ModifyStamina(float targetValue)
    {
        targetStaminaValue = targetValue;
    }

    private void Update()
    {
        currentStaminaValue =
            Mathf.SmoothDamp(currentStaminaValue, targetStaminaValue, ref staminaSmoothVelocity, 0.2f);
        staminaSlider.normalizedValue = currentStaminaValue;
    }
}
