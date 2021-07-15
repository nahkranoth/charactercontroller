using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class ZombieIdleState: AbstractEnemyState
{
    private float roamChance = 0.1f;
    private Vector3 roamTarget;
    private Coroutine idleWaitTimer;

    public ZombieIdleState()
    {
    }

    public override void Activate()
    { 
        idleWaitTimer = Parent.StartCoroutine(WaitToRoamAgain());
    }

    public override void Execute()
    {
        Parent.rigidBody.velocity = Vector2.zero;
        Parent.rigidBody.Sleep();
    }

    IEnumerator WaitToRoamAgain()
    {
        yield return new WaitForSeconds(2f);
        var newPath = Parent.pathfinding.FindPathToRandomPosByWorldPos(Parent.transform.position);
        Parent.npcPathController.InitializePath(newPath);
        Parent.npcPathController.NextNode();
        roamTarget = Parent.npcPathController.GetTarget();
        Parent.SetState("roam");
    }
    
}
