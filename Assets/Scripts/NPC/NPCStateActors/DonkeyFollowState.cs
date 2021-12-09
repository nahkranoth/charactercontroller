using System.Collections;
using UnityEngine;

public class DonkeyFollowState: AbstractNPCState
{
    private PlayerController player;
    private float currentWalkSpeed = 0.1f;
    private float resetFollowTime = 2f;
    private float resetFollowTimer = 0f;
    private INPCSettings settings;
    private Vector3 roamTarget;
    private Vector3 walkDirections;

    private float initializePathTimer = 0f;
    private float reInitializePathTime = 3f;

    public DonkeyFollowState(INPCSettings _settings)
    {
        settings = _settings;
    }
    
    public override void Activate()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        InitializePath();
    }

    public override void Deactivate()
    {
    }

    private void InitializePath()
    {
        if (initializePathTimer < reInitializePathTime) return;
        initializePathTimer = 0;
        var newPath = Parent.pathfinding.FindPathToTargetByWorldPos(Parent.transform.position, player.transform.position);
        Parent.npcPathController.InitializePath(newPath);
        Parent.npcPathController.NextNode();
        roamTarget = Parent.npcPathController.GetTarget();
    }

    public override void Execute()
    {
        resetFollowTimer += Time.deltaTime;
        initializePathTimer += Time.deltaTime;
        
        //if close enough to player, just go idle
        if (Helpers.InRange(Parent.transform.position, player.transform.position, .4f))
        {
            Parent.SetState("idle");
            return;
        }
        
        //Roam to player target
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
            resetFollowTimer = 0;
        }
        
        //Spawn to player if too far away
        if (!Helpers.InRange(Parent.transform.position, player.transform.position, 16f))
        {
            Parent.transform.position = player.transform.position + Vector3.down*1f;
            InitializePath();
        }

        if (resetFollowTimer >= resetFollowTime)
        {
            Parent.rigidBody.velocity = Vector3.zero;
            InitializePath();
        }
        
        SetVelocity();
    }
    
    private void SetVelocity()
    {
        var walkDirections = Parent.npcPathController.FindDeltaVecOfCurrentNode(Parent.transform.position);
        Parent.rigidBody.velocity = Vector3.Normalize(walkDirections) * settings.WalkSpeed;
        
        if (Mathf.Abs(walkDirections.x) > Mathf.Abs(walkDirections.y))
        {
            Parent.animatorController.SetWalk((int)Mathf.Sign(walkDirections.x), 0);
        }
        else
        {
            Parent.animatorController.SetWalk(0,(int)Mathf.Sign(walkDirections.y));
        }
        
        
    }
}