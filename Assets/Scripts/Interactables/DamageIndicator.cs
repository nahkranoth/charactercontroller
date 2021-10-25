using System;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public TextMesh damageText;
    public Animator damageAnimation;
    
    public InteractionDamageTaker damageIndicator;

    private void Start()
    {
        damageIndicator.OnTakeDamage -= ShowDamage;
        damageIndicator.OnTakeDamage += ShowDamage;
    }

    public void ShowDamage(int damage)
    {
        damageAnimation.SetTrigger("Hit");
        damageText.text = damage.ToString();
    }

}