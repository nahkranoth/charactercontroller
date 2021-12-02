using System;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public TextMesh damageText;
    public Animator damageAnimation;
    
    public InteractionDamageTaker damageIndicator;

    private void Start()
    {
        damageIndicator.OnInteraction -= ShowDamage;
        damageIndicator.OnInteraction += ShowDamage;
    }

    public void ShowDamage(int damage, PlayerToolActionType type)
    {
        if (type != PlayerToolActionType.Slash) return;
        damageAnimation.SetTrigger("Hit");
        damageText.text = damage.ToString();
    }

}
