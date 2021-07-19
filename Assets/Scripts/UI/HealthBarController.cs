using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Text healthText;
    private PlayerController player;

    private void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        SetHealth(player.Health, player.CurrentHealth);
        player.OnHealthChange -= SetHealth;
        player.OnHealthChange += SetHealth;
    }

    private void SetHealth(int health)
    {
        SetHealth(health, player.Health);
    }
    
    public void SetHealth(int currentHealth, int health)
    {
        healthText.text = $"{currentHealth}/{health}";
    }
}
