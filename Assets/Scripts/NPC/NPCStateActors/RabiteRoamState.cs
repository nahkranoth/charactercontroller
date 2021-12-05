using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class RabiteRoamState: AbstractNPCState
{
    private bool moving = false;
    private Vector3 roamTarget;
    private PlayerController player;
    private LevelEntityPlacer levelEntityPlacer;

    private INPCSettings settings;
    
    public RabiteRoamState(INPCSettings _settings)
    {
        settings = _settings;
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        levelEntityPlacer = WorldGraph.Retrieve(typeof(LevelEntityPlacer)) as LevelEntityPlacer;
    }

    public override void Activate()
    {
        var newPath = Parent.pathfinding.FindPathToRandomPosByWorldPos(Parent.transform.position, settings.maxRoam);
        Parent.npcPathController.InitializePath(newPath);
        Parent.npcPathController.NextNode();
        
        moving = false;
        Parent.rigidBody.velocity = Vector2.zero;
        roamTarget = Parent.npcPathController.GetTarget();
        
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
        IsInTargetDistance();
        
        if (Helpers.InRange(Parent.transform.position, roamTarget, .2f))
        {
            if (Parent.npcPathController.NextNode())
            {
                roamTarget = Parent.npcPathController.GetTarget();
            }
            else
            {
                Parent.rigidBody.velocity = Vector3.zero;
                Parent.animatorController.SetWalk(0, 0);
                Parent.SetState("idle");
                return;
            }
        }
        SetVelocity();
    }

    private void SetVelocity()
    {
        var walkDirections = Parent.npcPathController.FindDeltaVecOfCurrentNode(Parent.transform.position);
        Parent.rigidBody.velocity = Vector3.Normalize(walkDirections) * settings.roamWalkSpeed;
        Parent.animatorController.SetWalk((int)Mathf.Sign(-walkDirections.x), (int)Mathf.Sign(-walkDirections.y));
    }
    
}
