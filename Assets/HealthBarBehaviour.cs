using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    public Slider slider;
    public Color low;
    public Color high;
    public Vector3 offset;

    public void SetHealth(float health, float maxHealth)
    {
     
        slider.gameObject.SetActive(health < maxHealth);

        slider.value = health;
        slider.maxValue = maxHealth;


        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, health / maxHealth);
    }

    void Update()
    {

        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
     
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}