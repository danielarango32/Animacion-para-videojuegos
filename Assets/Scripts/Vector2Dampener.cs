using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector2Dampener
{
    private Vector2 currentValue;
    private Vector2 currentVelocity;

    private float dampTime;
    private float maxSpeed;

    public Vector2Dampener(float dampTime, float maxSpeed)
    {
        this.dampTime = dampTime;
        this.maxSpeed = maxSpeed;
    }

    public void Update ()
    {
        currentValue = Vector2.SmoothDamp(currentValue, TargetValue,ref currentVelocity, dampTime, maxSpeed);

    }

    public Vector2 CurrentValue => currentValue;    
    public Vector2 TargetValue { get; set; }
}
