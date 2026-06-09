using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSwim : MonoBehaviour
{
    [Header("Health Settings")]
    public float playerHP = 100f;
    public float maxPlayerHP = 100f;
    public Image healthBarFill;

    [Header("Shooting Settings")]
    public GameObject spearPrefab;   
    public Transform firePoint;       
    public float launchForce = 20f;
    public float attackRate = 1f;
    private float nextAttackTime = 5f;

    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float swimSpeed = 6f;
    public float jumpForce = 12f;

    [Header("State")]
    public bool isSwimming = true;

    private float horizontalInput;
    private float verticalInput;
    private bool isFacingRight = true;
    private bool isGrounded = true;

    private Rigidbody2D rb;
    private Animator playerAnimator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        rb.freezeRotation = true;

       
        playerHP = maxPlayerHP;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        playerHP -= damage;
        playerHP = Mathf.Clamp(playerHP, 0, maxPlayerHP); 

        UpdateHealthUI();
        Debug.Log("Ouch HAHAHHA! HP: " + playerHP);

        if (playerHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        FindObjectOfType<GameManager>().TriggerGameOver();

     
        gameObject.SetActive(false);
    }

    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
        
            healthBarFill.fillAmount = playerHP / maxPlayerHP;
        }
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;


        if (mousePos.x > transform.position.x && !isFacingRight) Flip();
        else if (mousePos.x < transform.position.x && isFacingRight) Flip();

 
        if (!isSwimming && Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            if (playerAnimator != null) playerAnimator.SetBool("isJumping", true);
        }


        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ShootSpear(mousePos);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void ShootSpear(Vector3 mousePos)
    {
        if (spearPrefab != null && firePoint != null)
        {
     
            Vector2 direction = ((Vector2)mousePos - (Vector2)firePoint.position).normalized;

     
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

       
            GameObject projectile = Instantiate(spearPrefab, firePoint.position, rotation);

            
            Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
            if (rbProjectile != null)
            {
                rbProjectile.linearVelocity = direction * launchForce;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isSwimming)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(horizontalInput * swimSpeed, verticalInput * swimSpeed);
        }
        else
        {
            rb.gravityScale = 3f;
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
            playerAnimator.SetFloat("yVelocity", rb.linearVelocity.y);
            playerAnimator.SetBool("isSwimming", isSwimming);
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
            if (playerAnimator != null) playerAnimator.SetBool("isJumping", false);
        }
    }
}