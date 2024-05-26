using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private float damage;
    private bool canMakeDamage=false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Canon")&& canMakeDamage)
        {
            Canon can = other.GetComponent<Canon>();
            canMakeDamage = false;
            can.TakeDamage(damage);
        }
    }

    public void ResetCanMakeDamage()
    {
        canMakeDamage = true;
        Debug.Log("Me reseteo");
    }
}
