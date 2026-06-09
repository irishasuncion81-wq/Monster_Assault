using UnityEngine;

public class arrow : MonoBehaviour
{
    GameObject target;
    public float speed = 10f; 
    Rigidbody2D arrowRB;

    void Start()
    {
        arrowRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");

        if (target != null)
        {
 
            Vector3 direction = target.transform.position - transform.position;

    
            Vector2 moveDir = new Vector2(direction.x, direction.y).normalized;

            arrowRB.linearVelocity = moveDir * speed;

 
            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


        Destroy(gameObject, 2f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(5f); 
            }
            Destroy(gameObject); 
        }
    }
}