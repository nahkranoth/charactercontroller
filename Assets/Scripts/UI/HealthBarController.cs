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
        player.OnDamage -= OnDamage;
        player.OnDamage += OnDamage;
    }

    private void OnDamage(int damage)
    {
        SetHealth(player.CurrentHealth - damage, player.Health);
    }
    
    public void SetHealth(int currentHealth, int health)
    {
        healthText.text = $"{currentHealth}/{health}";
    }
}
