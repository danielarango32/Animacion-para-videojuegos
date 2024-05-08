using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IKStopEnemy : MonoBehaviour
{
    private const float MAX_STOPPER_DISTANCE = 2.0f;
    
    [SerializeField] private Transform[] ikStops;
    [SerializeField] private float stopTime;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IIkTip ikTip))
        {
            ContactPoint contact = collision.contacts[0];
            Transform nearestHitStopper = ikStops.OrderBy(stop =>
            {
                float normalizedDistance = Mathf.Min(1.0f,
                    1 - (Vector3.Distance(contact.point, stop.position) / MAX_STOPPER_DISTANCE));
                return normalizedDistance;
            }).First();

            ikTip.Stop(nearestHitStopper, stopTime);   
        }
    }

    
}
