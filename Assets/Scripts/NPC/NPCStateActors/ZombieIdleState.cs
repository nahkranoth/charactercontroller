using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class ZombieIdleState: AbstractNPCState
{
    private float roamChance = 0.1f;
    private Vector3 roamTarget;
    
    private INPCSettings settings;
    private int idleWaitTimer;
    private bool waitingToRoam;
    
    private PlayerController player;
    private LevelEntityPlacer levelEntityPlacer;

    public ZombieIdleState(INPCSettings _settings)
    {
        settings = _settings;
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        levelEntityPlacer = WorldGraph.Retrieve(typeof(LevelEntityPlacer)) as LevelEntityPlacer;
    }

    public override void Activate()
    {
        roamChance = Random.Range(settings.roamChance.x, settings.roamChance.y);
        idleWaitTimer = 0;
        waitingToRoam = true;
    }

    public override void Deactivate()
    {
    }

    
    private void IsInTargetDistance()
    {
        foreach (var targetable in levelEntityPlacer.enemyTargetPool)
        {
            if (Vector3.Distance(targetable.GetTransform().position, Parent.transform.position) < settings.detectDistance)
            {
                Parent.attackTarget = targetable.GetTransform();
                Parent.SetState("angry");
            }
        }
    }
    
    public override void Execute()
    {
        Parent.rigidBody.velocity = Vector2.zero;
        Parent.rigidBody.Sleep();

        if (waitingToRoam)
        {
            idleWaitTimer++;
            if (idleWaitTimer > roamChance*100)
            {
                Parent.SetState("roam");
                waitingToRoam = false;
            }
        }

        IsInTargetDistance();
    }
    
}
