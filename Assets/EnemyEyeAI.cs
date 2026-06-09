using UnityEngine;
using System.Collections;

public class EnemyEyeAI : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageAmount = 20; 

    [Header("Movement Settings")]
    public float followSpeed = 3f;
    public float dashSpeed = 20f;
    public float stopDistance = 2f;

    [Header("Attack Settings")]
    public float attackRange = 7f;
    public float attackCooldown = 3f;

    [Header("Sprite Orientation")]
    public bool isFacingRightByDefault = false;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private float nextAttackTime;
    private bool isDashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

 
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player missing.");
        }


        rb.gravityScale = 0;

    }

    void Update()
    {
        if (player == null || isDashing) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            FollowPlayer();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            StartCoroutine(DashAttack());
            nextAttackTime = Time.time + attackCooldown;
        }


        FlipTowardsPlayer();
    }

    void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * followSpeed;
    }

    void FlipTowardsPlayer()
    {
    
        if (player.position.x > transform.position.x)
        {
      
            transform.localScale = isFacingRightByDefault ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        }
        else
        {
     
            transform.localScale = isFacingRightByDefault ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
        }
    }

    IEnumerator DashAttack()
    {
        isDashing = true;

    
        rb.linearVelocity = Vector2.zero;

 
        if (anim != null) anim.SetTrigger("isAttacking");

  
        yield return new WaitForSeconds(0.8f);


        Vector2 dashDir = (player.position - transform.position).normalized;
        rb.linearVelocity = dashDir * dashSpeed;

 
        yield return new WaitForSeconds(0.4f);

  
        Debug.Log("Dash");

     
        Vector2 retreatDir = -dashDir;
        rb.linearVelocity = retreatDir * (dashSpeed * 0.6f);


        yield return new WaitForSeconds(0.5f);


        rb.linearVelocity = Vector2.zero;
        isDashing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
      
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("The player got damage");
            }
        }
    }
}