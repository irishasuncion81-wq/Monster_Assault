using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Basic Settings")]
    public float playerScale = 2.5f;
    public float moveSpeed = 10f;
    public float jumpForce = 12f;
    public float normalGravity = 2.5f;

    [Header("Swimming Settings")]
    public float swimSpeed = 7f;

    [Header("States (Check these in Inspector)")]
    public bool isSwimming = false;
    public bool isGrounded = false;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        transform.localScale = new Vector3(playerScale, playerScale, 1f);
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (isSwimming)
        {
            // --- SWIMMING MODE ---
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(h * swimSpeed, v * swimSpeed);
        }
        else
        {
            // --- LAND MODE ---
            rb.gravityScale = normalGravity;
 
            rb.linearVelocity = new Vector2(h * moveSpeed, rb.linearVelocity.y);


            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }

        if (h != 0) transform.localScale = new Vector3(Mathf.Sign(h) * playerScale, playerScale, 1f);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water")) isSwimming = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water")) isSwimming = false;
    }

    private void OnCollisionStay2D(Collision2D col)
    {
       
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}