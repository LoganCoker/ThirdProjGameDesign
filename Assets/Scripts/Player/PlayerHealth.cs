using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthBar;
    public float maxHealth = 100f;
    public float currentHealth;

    private float hitCooldown;
    private PlayerInMove playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        playerMovement = GetComponent<PlayerInMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar.value != currentHealth/maxHealth)
        {
            healthBar.value = currentHealth/maxHealth;
        }
        
        hitCooldown -= Time.deltaTime;
    }

    void takedamage(float damage)
    {
        currentHealth -= damage;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("BossAttack") && hitCooldown <= 0) {
            playerMovement.WasHit = other.transform.parent.parent.forward.normalized;
            takedamage(5f);
            hitCooldown = 2f;
        }
    }
}