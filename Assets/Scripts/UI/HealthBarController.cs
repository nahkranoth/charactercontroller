using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Text healthText;
    private PlayerController player;

    private void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        SetHealth(player.stateController.CurrentHealth, player.stateController.MaxHealth);
        player.stateController.OnChangeHealth -= SetHealth;
        player.stateController.OnChangeHealth += SetHealth;
    }

    private void SetHealth(int amount)
    {
        SetHealth(amount, player.stateController.MaxHealth);
    }
    
    public void SetHealth(int currentHealth, int maxHealth)
    {
        healthText.text = $"{currentHealth}/{maxHealth}";
    }
}
