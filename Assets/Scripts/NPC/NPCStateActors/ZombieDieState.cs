using System.Collections;
using UnityEngine;

public class ZombieDieState: AbstractNPCState
{
    public ZombieDieState()
    {
    }
    
    public override void Activate()
    {
        Parent.animatorController.SetWalk(0,0);
        Parent.animatorController.SetDie();
        Parent.StartCoroutine(StartRemovalSequence());
    }

    public override void Deactivate()
    {
    }

    IEnumerator StartRemovalSequence()
    {
        yield return new WaitForSeconds(3f);
        Parent.DestroyMe();
    }
    
    public override void Execute()
    {
        Parent.rigidBody.velocity = Vector2.zero;
    }
}