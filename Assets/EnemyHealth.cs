using UnityEngine;
using UnityEngine.UI; // IMPORTANTE: Para gumana ang Image component

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Settings")]
    public Image healthBarFill; // I-drag dito yung Fill Image mula sa Enemy Canvas

    private EnemySpawner mySpawner;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar(); 
    }

    public void SetSpawner(EnemySpawner spawner)
    {
        mySpawner = spawner;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar(); 

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }
    void Die()
    {
        
        GameManager gm = Object.FindFirstObjectByType<GameManager>();

        if (gm != null)
        {
            gm.AddKill(); 
        }

        if (mySpawner != null) mySpawner.EnemyDied();
        Destroy(gameObject);

        Destroy(gameObject); 
    }

    public void OnAnimationEventTriggered() { }
    public void Attack() { }
}