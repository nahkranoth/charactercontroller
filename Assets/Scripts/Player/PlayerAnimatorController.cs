using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
   public Animator animator;

   public void SetWalk(int dirX, int dirY)
   {
      animator.SetInteger("DirectionX", dirX);
      animator.SetInteger("DirectionY", dirY);
   }

   public void AttackSlash()
   {
      animator.SetTrigger("AttackSlash");
   }

   public void SetDamage()
   {
      animator.SetTrigger("Damage");
   }
   
   public void SetCharging(bool charging)
   {
      animator.SetBool("Charging", charging);
   }
}
