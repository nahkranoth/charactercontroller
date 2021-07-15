﻿using UnityEngine;

public class KnockbackHitState: AbstractEnemyState
{
    public KnockbackHitState(float _knockbackAmount)
    {
    }
    
    public override void Activate()
    {
        Parent.animatorController.SetWalk(0,0);
    }

    public override void Execute()
    {
        Parent.rigidBody.velocity = Vector2.zero;
    }
}
