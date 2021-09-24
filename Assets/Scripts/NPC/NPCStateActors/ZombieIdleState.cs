using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class ZombieIdleState: AbstractEnemyState
{
    private float roamChance = 0.1f;
    private Vector3 roamTarget;
    
    private INPCSettings settings;
    private Coroutine idleWaitTimer;
    
    private PlayerController player;

    public ZombieIdleState(INPCSettings _settings)
    {
        settings = _settings;
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
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
        if (Vector3.Distance(player.transform.position, Parent.transform.position) < settings.detectDistance)
        {
            Parent.StopCoroutine(WaitToRoamAgain());
            Parent.SetState("angry");
        }
    }

    IEnumerator WaitToRoamAgain()
    {
        yield return new WaitForSeconds(Random.Range(settings.roamChance.x, settings.roamChance.y));
        Parent.SetState("roam");
    }
    
}
