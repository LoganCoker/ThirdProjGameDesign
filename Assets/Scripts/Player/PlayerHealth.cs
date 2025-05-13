using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthBar;
    public float maxHealth = 100f;
    public float currentHealth;

    private float hitCooldown;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar.value != currentHealth)
        {
            healthBar.value = currentHealth;
        }

        hitCooldown -= Time.deltaTime;
    }

    void takedamage(float damage)
    {
        currentHealth -= damage;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Boss") && hitCooldown <= 0) {
            takedamage(1f);
            hitCooldown = 2f;
        }
    }
}