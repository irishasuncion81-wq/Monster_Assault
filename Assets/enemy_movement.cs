using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    [Header("Health Settings")]
    public float hitpoints; 
    public float maxHitpoints = 100f; 

    public Image healthBarFill;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float patrolDuration = 3f;
    private float patrolTimer;
    private bool movingRight = true;

    [Header("Detection Settings")]
    public Transform player;
    public float detectionRange = 10f; 
    public float shootingRange = 5f;  

    [Header("Attack Settings")]
    public GameObject arrowPrefab;
    public Transform firePoint;      
    public float attackRate = 1f;
    private float nextAttackTime;

    [Header("Edge Detection")]
    public Transform groundCheck;
    public float rayDistance = 1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        patrolTimer = patrolDuration;
        hitpoints = maxHitpoints;
        UpdateHealthUI();
  
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        if (rb != null) rb.freezeRotation = true;
    }
    public void TakeDamage(float damage)
    {
        hitpoints -= damage;
        hitpoints = Mathf.Clamp(hitpoints, 0, maxHitpoints);

        UpdateHealthUI(); 

        if (hitpoints <= 0)
        {
            Die();
        }
    }


    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = hitpoints / maxHitpoints;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        if (player == null)
        {
            Patrol();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= shootingRange)
        {
  
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRange)
        {
          
            ChasePlayer();
        }
        else
        {
            
            Patrol();
        }
    }

    void Patrol()
    {
        if (anim != null)
        {
            anim.SetBool("isAttacking", false);
            anim.SetFloat("Speed", 1f);
        }

        patrolTimer -= Time.deltaTime;
        if (patrolTimer <= 0)
        {
            movingRight = !movingRight;
            patrolTimer = patrolDuration;
        }

        if (!IsGroundAhead())
        {
            movingRight = !movingRight;
            patrolTimer = patrolDuration;
        }

        float direction = movingRight ? 1 : -1;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        UpdateFacing(direction);
    }


    void ChasePlayer()
    {
        float direction = (player.position.x > transform.position.x) ? 1 : -1;

        
        if (IsGroundAhead())
        {
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
            if (anim != null) anim.SetFloat("Speed", 1f);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Hinto sa bangin
            if (anim != null) anim.SetFloat("Speed", 0f);
        }

        UpdateFacing(direction);


        if (anim != null)
        {
            anim.SetBool("isAttacking", false);
            anim.SetFloat("Speed", 1f);
        }
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        UpdateFacing(direction);
    }
    void AttackPlayer()
    {
        // Stop movement
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (anim != null)
        {
            anim.SetFloat("Speed", 0f);
        }

        float direction = (player.position.x > transform.position.x) ? 1 : -1;
        UpdateFacing(direction);

        // Shooting logic
        if (Time.time >= nextAttackTime)
        {
            if (anim != null) anim.SetBool("isAttacking", true);

            Shoot();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void Shoot()
    {
        if (arrowPrefab != null && firePoint != null)
        {
            Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        }
    }

    void UpdateFacing(float direction)
    {
        if (direction > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (direction < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }

    bool IsGroundAhead()
    {
        if (groundCheck == null) return true;
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, rayDistance, groundLayer);
        return hit.collider != null;
    }
}