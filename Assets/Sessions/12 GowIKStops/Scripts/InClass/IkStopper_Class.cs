using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkStopper_Class : MonoBehaviour
{
    [SerializeField] private float stopDelay = 0.6f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IkWeapon_Class weapon))
        {
            weapon.Stop(stopDelay);
        }
    }
}
