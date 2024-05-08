using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour, IPickable
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IPicker picker))
        {
            ((IPickable)this).RegisterAvailability(picker);
        }
    }

    public Vector3 Position => transform.position;
}
