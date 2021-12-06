using System;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public TextMesh damageText;
    public Animator damageAnimation;
    
    public NPCDamageInteraction handler;

    private void Start()
    {
        handler.OnDamage -= ShowDamage;
        handler.OnDamage += ShowDamage;
    }

    public void ShowDamage(int damage)
    {
        if (damage <= 0) return;
        damageAnimation.SetTrigger("Hit");
        damageText.text = damage.ToString();
    }

}
