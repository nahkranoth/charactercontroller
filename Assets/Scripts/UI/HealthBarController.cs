using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Text healthText;
    private PlayerController player;

    private void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        SetHealth(player.statusController.CurrentHealth, player.statusController.MaxHealth);
        player.statusController.OnChangeHealth -= SetHealth;
        player.statusController.OnChangeHealth += SetHealth;
    }

    private void SetHealth(int amount)
    {
        SetHealth(amount, player.statusController.MaxHealth);
    }
    
    public void SetHealth(int currentHealth, int maxHealth)
    {
        healthText.text = $"{currentHealth}/{maxHealth}";
    }
}
