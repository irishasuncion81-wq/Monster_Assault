using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 30f;
    public float damage = 10f;
    public float lifetime = 2f;
    private Rigidbody2D rb;

    private bool hasDealtDamage = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        if (hasDealtDamage) return;

        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                hasDealtDamage = true; 
                enemy.TakeDamage(damage); 

                Debug.Log("Bullet hit: " + collision.gameObject.name);
            }

            Destroy(gameObject);
        }
    }
}