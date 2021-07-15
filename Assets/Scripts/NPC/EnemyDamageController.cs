using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class EnemyDamageController : MonoBehaviour
{
    public TextMesh damageText;
    public Animator damageAnimation;

    public void ShowDamage(int damage)
    {
        damageAnimation.SetTrigger("Hit");
        damageText.text = damage.ToString();
    }

}
