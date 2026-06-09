using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class SlimeAI : MonoBehaviour
{
    [Header("Health Settings")]
    public float hitpoints;
    public float maxHitpoints = 100f;
    public Image healthBarFill; 

    [Header("Movement & Attack")]
    public Transform player;
    public float chaseSpeed = 3f;
    public float attackRange = 1.2f;
    public int attackDamage = 20;

    [Header("Eat & Spit Settings")]
    public float digestionTime = 1.5f;
    public float spitForce = 20f;
    public float stunDuration = 0.5f;

    private Animator anim;
    private Rigidbody2D rb;
    private bool isAttacking = false;
    private Rigidbody2D playerRb;
    private PlayerSwim playerMovementScript;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        hitpoints = maxHitpoints;
        UpdateHealthUI();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerRb = playerObj.GetComponent<Rigidbody2D>();
            playerMovementScript = playerObj.GetComponent<PlayerSwim>();
        }

        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.freezeRotation = true;
        }
    }

    public void TakeDamage(float damage)
    {
        hitpoints -= damage;
        hitpoints = Mathf.Clamp(hitpoints, 0, maxHitpoints);
        UpdateHealthUI();

        if (hitpoints <= 0)
        {
            StopAllCoroutines();
            ReleasePlayer();
            Die();
        }
    }

    void ReleasePlayer()
    {
        if (player != null && player.parent == transform)
        {
            player.SetParent(null);
            player.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            if (playerMovementScript != null) playerMovementScript.enabled = true;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = hitpoints / maxHitpoints;
        }
    }

    void Update()
    {
        if (player == null || isAttacking) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < 10f && distance > attackRange)
        {
            ChasePlayer();
        }
        else if (distance <= attackRange && !isAttacking)
        {
            StartCoroutine(EatAndSpit());
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * chaseSpeed;

        if (direction.x > 0) transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        else transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
    }

    IEnumerator EatAndSpit()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        if (anim != null) anim.SetTrigger("isAttacking");

        if (playerMovementScript != null)
        {
            playerMovementScript.TakeDamage(15);
            playerMovementScript.enabled = false;
        }

        player.SetParent(transform);
        player.localPosition = Vector3.zero;
        player.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(digestionTime);

        player.SetParent(null);
        player.gameObject.GetComponent<SpriteRenderer>().enabled = true;

        if (playerMovementScript != null)
        {
            playerMovementScript.TakeDamage(5);
        }

        Vector2 spitDirection = (player.position - transform.position).normalized;
        if (spitDirection == Vector2.zero) spitDirection = Vector2.up;

        playerRb.linearVelocity = Vector2.zero;
        playerRb.AddForce(spitDirection * spitForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(stunDuration);

        if (playerMovementScript != null) playerMovementScript.enabled = true;

        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }
}