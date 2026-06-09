using UnityEngine;

public class WaterSpearProjectile : MonoBehaviour
{
    public float damageValue = 20f;
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Spear hit: " + collision.gameObject.name + " | Tag: " + collision.gameObject.tag);

 
        if (collision.CompareTag("Enemy"))
        {
    
            SlimeAI slime = collision.GetComponentInParent<SlimeAI>();

            if (slime != null)
            {
                slime.TakeDamage(damageValue);
                Debug.Log("<color=green>Hit Slime!</color>");
                Destroy(gameObject); 
            }
        }
        else if (collision.CompareTag("Ground") || collision.gameObject.name.ToLower().Contains("wall"))
        {
            Destroy(gameObject);
        }
    }
}