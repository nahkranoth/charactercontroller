using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Text healthText;
    public Health health;

    private void Start()
    {
        SetHealth(health.Amount, health.MaxAmount);
        health.OnChange -= SetHealth;
        health.OnChange += SetHealth;
    }

    private void SetHealth(int amount)
    {
        SetHealth(amount, health.MaxAmount);
    }
    
    public void SetHealth(int currentHealth, int maxHealth)
    {
        healthText.text = $"{currentHealth}/{maxHealth}";
    }
}
