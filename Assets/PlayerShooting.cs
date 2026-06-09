using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float bulletSpeed = 30f;

    private float nextFireTime;
    private Animator anim;
    private Camera mainCam;

    void Start()
    {
        anim = GetComponent<Animator>();
        mainCam = Camera.main; 

        if (mainCam == null)
        {
            Debug.LogError("No maincam");
        }
    }

    void Update()
    {

        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return; 
            }

            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
     
        if (bulletPrefab == null || firePoint == null || mainCam == null) return;

   
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 direction = (mousePos - firePoint.position).normalized;

    
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

   
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);

    
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        if (bulletRB != null)
        {
            bulletRB.linearVelocity = direction * bulletSpeed;
        }

        if (anim != null)
        {
            anim.SetTrigger("shoot");
        }

    }
}