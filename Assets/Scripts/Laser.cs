using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Laser : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f;
    private Vector3 direction;
    [SerializeField] CharacterDamage characterDamage;

    public void Initialize(Vector3 dir, CharacterDamage characterDamage)
    {
        direction = dir;
        this.characterDamage = characterDamage;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 characterSpacePosition = characterDamage.transform.InverseTransformPoint(transform.position).normalized;
            float x = characterSpacePosition.x;
            x = Mathf.Sign(x)*Mathf.Ceil(Mathf.Abs(x));

            float y = MathF.Abs(x)>=MathF.Abs(characterSpacePosition.z) ? 0: characterSpacePosition.z;
            y = Mathf.Sign(y)*Mathf.Ceil(Mathf.Abs(y));
            // Asumiendo que tu jugador tiene un componente de salud o un método para recibir daño
            characterDamage.RecieveDamage(10,new Vector2(x,y));
            Destroy(gameObject);
        }
    }
}
