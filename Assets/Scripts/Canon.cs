using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform firePoint;
    public float minFireRate = 1f;
    public float maxFireRate = 5f;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    [SerializeField] CharacterDamage characterDamage;
    [SerializeField] private float maxHealth;
    private float currentHealth;

    private Transform player;
    private Vector3 originalPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
        originalPosition = transform.localPosition;
        StartCoroutine(FireLaser());
    }
    void Update()
    {
        if (player != null)
        {
            // Hacer que el cañón apunte hacia el jugador
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(targetPosition);
        }
    }

    IEnumerator FireLaser()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minFireRate, maxFireRate));
            if(!characterDamage.Dead)
                Fire();
        }
    }

    void Fire()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - firePoint.position).normalized;
            GameObject laser = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
            laser.GetComponent<Laser>().Initialize(direction,characterDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth > 0)
        {
            StartCoroutine(Shake());
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomPoint = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = new Vector3(randomPoint.x, originalPosition.y, randomPoint.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
