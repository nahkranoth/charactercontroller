using System.Collections;
using UnityEngine;

public class RabiteAttackState: AbstractNPCState
{
    private PlayerController player;
    private INPCSettings settings;
    private Vector3 attackDirection;

    public RabiteAttackState(INPCSettings _settings)
    {
        settings = _settings;
    }
    
    public override void Activate()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        attackDirection = (Parent.attackTarget.position - Parent.transform.position).normalized;
        Attack();
    }

    public override void Deactivate()
    {
    }

    public override void Execute()
    {
        SetVelocity();
    }
    
    private void SetVelocity()
    {
        Parent.rigidBody.velocity = Vector3.Normalize(attackDirection) * settings.attackSpeed;
    }
    
    private void Attack()
    {
        Parent.animatorController.Attack();
        Parent.StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(.2f);
        Parent.attacking = true;
        yield return new WaitForSeconds(.5f);
        Parent.attacking = false;
        if (Parent.attackTarget == null)//I killed him!
        {
            Parent.SetState("roam");
        }
        else
        {
            Parent.SetState("angry");
        }
    }
}