using UnityEngine;
using UnityEngine.UI; 

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public Image healthBarFill;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar(); 
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
        gameObject.SetActive(false);
    }
}