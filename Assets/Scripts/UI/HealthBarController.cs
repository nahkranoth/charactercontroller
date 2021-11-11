using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Text healthText;
    private PlayerController player;

    private void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        SetHealth(player.status.CurrentHealth, player.status.MaxHealth);
        player.status.OnChange -= SetHealth;
        player.status.OnChange += SetHealth;
    }

    private void SetHealth(int amount)
    {
        SetHealth(amount, player.status.MaxHealth);
    }
    
    public void SetHealth(int currentHealth, int maxHealth)
    {
        healthText.text = $"{currentHealth}/{maxHealth}";
    }
}
