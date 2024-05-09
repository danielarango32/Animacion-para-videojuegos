using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDamage : MonoBehaviour, IDamageable
{
    [SerializeField] private Animator anim;
    [SerializeField] private float maxHealth;
    private float currentHealth;

    public void Awake()
    {
        currentHealth = maxHealth;
    }
    public void RecieveDamage(float damage, Vector2 direction)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            //Die
            //Ejecutar animacion de muerte
            //Desactivar el gameObject
            anim.SetTrigger("Die");
            return;
        }

        Debug.DrawLine(transform.position, transform.TransformPoint(new Vector3(direction.x, 0, direction.y)), Color.red, 5f);

        anim.SetFloat("DamageX", direction.x);
        anim.SetFloat("DamageY", direction.y);
        anim.SetTrigger("Damage");

    }
}   

