using System.Collections;
using UnityEngine;

public class NPCAnimatorController : MonoBehaviour
{
    public Animator animator;

    public void SetWalk(int dirX, int dirY)
    {
        animator.SetInteger("DirectionX", dirX);
        animator.SetInteger("DirectionY", dirY);
    }

    public void SetDie()
    {
        animator.SetTrigger("Die");
    }

    public void Damage()
    {
        animator.SetTrigger("Damage");
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
    
    public void TelegraphAttack()
    {
        animator.SetTrigger("Telegraph");
    }

}