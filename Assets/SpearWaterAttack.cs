using UnityEngine;

public class SpearWaterAttack : MonoBehaviour
{
    [Header("Spear Settings")]
    public GameObject waterSpearPrefab; 
    public Transform waterFirePoint;    
    public float spearSpeed = 20f;
    public float attackCooldown = 1.2f;
    private float nextAttackTime = 0f;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime)
        {
            Debug.Log("Water Spear Attack Triggered!");
            ShootSpear();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void ShootSpear()
    {
  
        if (anim != null)
        {
            anim.SetTrigger("isShooting");
        }

        GameObject spear = Instantiate(waterSpearPrefab, waterFirePoint.position, Quaternion.identity);


        Rigidbody2D rb = spear.GetComponent<Rigidbody2D>();
        if (rb != null)
        {

            float direction = transform.localScale.x > 0 ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * spearSpeed, 0);

         
            if (direction < 0)
            {
                Vector3 s = spear.transform.localScale;
                s.x *= -1;
                spear.transform.localScale = s;
            }
        }

    
        Destroy(spear, 2f);
    }
}