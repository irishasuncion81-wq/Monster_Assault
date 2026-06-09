using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public float bounceForce = 15f;

    [Header("Game Over Settings")]
    public GameObject gameOverCanvas;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;     
    public float bulletSpeed = 20f;

    private float horizontalInput;
    private bool isFacingRight = true;
    private bool isGrounded = true;
    private bool canDoubleJump = false;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

       
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

     
        if (mousePosition.x > transform.position.x && !isFacingRight) Flip();
        else if (mousePosition.x < transform.position.x && isFacingRight) Flip();


        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded) { Jump(); canDoubleJump = true; }
            else if (canDoubleJump) { DoubleJump(); }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.K))
        {
            Shoot(mousePosition);
        }
    }

    void Shoot(Vector3 targetPos)
    {
        if (animator != null) animator.SetTrigger("isShooting");

        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
       
                Vector2 direction = (targetPos - firePoint.position).normalized;

        
                bulletRb.linearVelocity = direction * bulletSpeed;

     
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
        if (animator != null)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isGrounded", false);
        }
    }

    void DoubleJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        canDoubleJump = false;
        if (animator != null)
        {
            animator.SetTrigger("isDoubleJumping");
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        if (animator != null)
        {
            float speedValue = isGrounded ? Mathf.Abs(rb.linearVelocity.x) : 0f;
            animator.SetFloat("xVelocity", speedValue);
            animator.SetBool("isGrounded", isGrounded);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 ls = transform.localScale;
        ls.x *= -1f;
        transform.localScale = ls;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canDoubleJump = false;
            if (animator != null) animator.SetBool("isGrounded", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyHead"))
        {
            EnemyHealth enemyHealth = collision.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(999f);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
            }
        }


        if (collision.CompareTag("Finish"))
        {
            GameOver();
        }
    }

    void GameOver()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true); 
            Time.timeScale = 0f;           
            Debug.Log("Player fell! Game Over.");
        }
    }
}