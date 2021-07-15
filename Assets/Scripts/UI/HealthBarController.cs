using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Text healthText;
    public PlayerController player;

    private void Start()
    {
        SetHealth(player.Health);
        player.OnDamage -= OnDamage;
        player.OnDamage += OnDamage;
    }

    private void OnDamage(int damage)
    {
        SetHealth(player.Health - damage);
    }
    
    public void SetHealth(int health)
    {
        healthText.text = health.ToString();
    }
}
