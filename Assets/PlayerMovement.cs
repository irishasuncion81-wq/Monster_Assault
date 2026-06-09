using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    private float horizontalInput;
    private bool isFacingRight = true;
    private bool isGrounded = true;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Siguraduhin na hindi tutumba ang character
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        // 1. Get Input
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. Flip Sprite Logic
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }

        // 3. Jump Logic (Tatalon lang kung nasa lupa)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;

            // Trigger jump animation agad
            if (animator != null)
            {
                animator.SetBool("isJumping", true);
            }
        }
    }

    [Header("Stomp Settings")]
    public float bounceForce = 15f; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
    
        if (collision.CompareTag("EnemyHead"))
        {

            EnemyHealth enemyHealth = collision.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(999f);

       
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
          
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
                }

                Debug.Log("Na-stomp ang enemy!");
            }
        }

        if (collision.CompareTag("Finish"))
        {
            Debug.Log("Nabangga ang Finish Line!"); 

            LevelManager manager = FindObjectOfType<LevelManager>();
            if (manager != null)
            {
                manager.ShowLevelComplete();
            }
        }
    }

    private void FixedUpdate()
    {
   
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

   
        if (animator != null)
        {
      
            animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));

       
            animator.SetFloat("yVelocity", rb.linearVelocity.y);
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

            if (animator != null)
            {
                animator.SetBool("isJumping", false);
            }
        }
    }
}