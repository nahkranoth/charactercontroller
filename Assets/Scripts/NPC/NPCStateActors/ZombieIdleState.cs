using System.Collections;
using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class ZombieIdleState: AbstractEnemyState
{
    private float roamChance = 0.1f;
    private Vector3 roamTarget;
    
    private INPCSettings settings;
    private Coroutine idleWaitTimer;

    public ZombieIdleState(INPCSettings _settings)
    {
        settings = _settings;
    }

    public override void Activate()
    { 
        if(idleWaitTimer != null) Parent.StopCoroutine(idleWaitTimer);
        idleWaitTimer = Parent.StartCoroutine(WaitToRoamAgain());
    }

    public override void Execute()
    {
        Parent.rigidBody.velocity = Vector2.zero;
        Parent.rigidBody.Sleep();
    }

    IEnumerator WaitToRoamAgain()
    {
        yield return new WaitForSeconds(Random.Range(settings.roamChance.x, settings.roamChance.y));
        Parent.SetState("roam");
    }
    
}
