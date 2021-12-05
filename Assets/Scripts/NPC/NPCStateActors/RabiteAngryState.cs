using System.Collections;
using UnityEngine;

public class RabiteAngryState: AbstractNPCState
{
    private float currentWalkSpeed = 0.1f;
    private bool attackAllowed;
    private float attackTimer = 0f;
    private INPCSettings settings;
    private Vector3 roamTarget;
    private Vector3 walkDirections;

    private bool refindPathAllowed = true;

    private PlayerController player;
    
    public RabiteAngryState(INPCSettings _settings)
    {
        settings = _settings;
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
    }
    
    public override void Activate()
    {
        InitializePath();
    }

    public override void Deactivate()
    {
    }

    private void InitializePath()
    {
        var newPath = Parent.pathfinding.FindPathToTargetByWorldPos(Parent.transform.position, Parent.attackTarget.position);
        Parent.npcPathController.InitializePath(newPath);
        Parent.npcPathController.NextNode();
        roamTarget = Parent.npcPathController.GetTarget();
    }

    public override void Execute()
    {
        if (!attackAllowed && Helpers.InRange(Parent.attackTarget.position, Parent.transform.position, settings.strikeDistance))
        {
            attackAllowed = true;
        }

        if (attackAllowed)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= settings.strikeDelayTime && Helpers.InRange(Parent.attackTarget.position, Parent.transform.position, settings.strikeDistance))
            {
                attackAllowed = false;
                attackTimer = 0f;
                Parent.SetState("attack");
            }
        }
        
        if (Vector3.Distance(Parent.attackTarget.position, Parent.transform.position) > settings.loseDistance)
        {
            Parent.animatorController.SetWalk(0, 0);
            Parent.SetState("idle");
            return;
        }
        
        if (Helpers.InRange(Parent.transform.position, roamTarget, .2f))
        {
            if (Parent.npcPathController.NextNode())
            {
                roamTarget = Parent.npcPathController.GetTarget();
            }
            else
            {
                Parent.rigidBody.velocity = Vector3.zero;
                InitializePath();
            }
        }
        SetVelocity();
    }
    
    private void SetVelocity()
    {
        var walkDirections = Parent.npcPathController.FindDeltaVecOfCurrentNode(Parent.transform.position);
        Parent.rigidBody.velocity = Vector3.Normalize(walkDirections) * settings.angryWalkSpeed;
        Parent.animatorController.SetWalk((int)Mathf.Sign(-walkDirections.x), (int)Mathf.Sign(-walkDirections.y));
    }
}