using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDamage : MonoBehaviour, IDamageable
{
    [SerializeField] private Slider uiHealthSlider;
    [SerializeField] private Animator anim;
    [SerializeField] private float maxHealth;
    [field: SerializeField] public bool Dead { get; private set; } = false;

    private float currentHealth;

    public void Awake()
    {
        currentHealth = maxHealth;
        uiHealthSlider.value = currentHealth;
    }
    public void RecieveDamage(float damage, Vector2 direction)
    {
        currentHealth -= damage;
        uiHealthSlider.value = currentHealth;
        if (currentHealth <= 0 && !Dead)
        {
            //Die
            //Ejecutar animacion de muerte
            //Desactivar el gameObject
            anim.SetTrigger("Die");
            Dead = true;
            return;
        }
        else if (currentHealth<=0)
        {
            return;
        }
        

        Debug.DrawLine(transform.position, transform.TransformPoint(new Vector3(direction.x, 0, direction.y)), Color.red, 5f);

        anim.SetFloat("DamageX", direction.x);
        anim.SetFloat("DamageY", direction.y);
        anim.SetTrigger("Damage");

    }
}   

