using UnityEngine;

public class KnockbackHitState: AbstractNPCState
{
    public KnockbackHitState(float _knockbackAmount)
    {
    }
    
    public override void Activate()
    {
        Parent.animatorController.SetWalk(0,0);
    }

    public override void Deactivate()
    {
    }

    public override void Execute()
    {
        Parent.rigidBody.velocity = Vector2.zero;
    }
}
