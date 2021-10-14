using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class ZombieIdleState: AbstractEnemyState
{
    private float roamChance = 0.1f;
    private Vector3 roamTarget;
    
    private INPCSettings settings;
    private int idleWaitTimer;
    private bool waitingToRoam;
    
    private PlayerController player;

    public ZombieIdleState(INPCSettings _settings)
    {
        settings = _settings;
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
    }

    public override void Activate()
    {
        roamChance = Random.Range(settings.roamChance.x, settings.roamChance.y);
        idleWaitTimer = 0;
        waitingToRoam = true;
    }

    public override void Execute()
    {
        Parent.rigidBody.velocity = Vector2.zero;
        Parent.rigidBody.Sleep();

        if (waitingToRoam)
        {
            idleWaitTimer++;
            if (idleWaitTimer > roamChance)
            {
                Parent.SetState("roam");
                waitingToRoam =false;
            }
        }
        
        if (Vector3.Distance(player.transform.position, Parent.transform.position) < settings.detectDistance)
        {
            Parent.SetState("angry");
        }
    }
    
}
