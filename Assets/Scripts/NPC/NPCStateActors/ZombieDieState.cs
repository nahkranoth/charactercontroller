using System.Collections;
using UnityEngine;

public class ZombieDieState: AbstractEnemyState
{
    public ZombieDieState()
    {
    }
    
    public override void Activate()
    {
        Parent.animatorController.SetWalk(0,0);
        Parent.animatorController.SetDie();
        Parent.mainHitbox.enabled = false;
        Parent.StartCoroutine(StartRemovalSequence());
    }

    IEnumerator StartRemovalSequence()
    {
        yield return new WaitForSeconds(3f);
        Parent.Destroy();
    }
    
    public override void Execute()
    {
        Parent.rigidBody.velocity = Vector2.zero;
    }
}