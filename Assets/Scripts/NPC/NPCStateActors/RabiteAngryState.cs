﻿using System.Collections;
using UnityEngine;

public class RabiteAngryState: AbstractNPCState
{
    private PlayerController player;
    private float currentWalkSpeed = 0.1f;
    private bool attackAllowed;
    private float attackTimer = 0f;
    private INPCSettings settings;
    private Vector3 roamTarget;
    private Vector3 walkDirections;

    private bool refindPathAllowed = true;
    public RabiteAngryState(INPCSettings _settings)
    {
        settings = _settings;
    }
    
    public override void Activate()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        InitializePath();
    }

    private void InitializePath()
    {
        var newPath = Parent.pathfinding.FindPathToPlayerByWorldPos(Parent.transform.position);
        Parent.npcPathController.InitializePath(newPath);
        Parent.npcPathController.NextNode();
        roamTarget = Parent.npcPathController.GetTarget();
    }

    public override void Execute()
    {
        if (!attackAllowed && Helpers.InRange(player.transform.position, Parent.transform.position, settings.strikeDistance))
        {
            attackAllowed = true;
        }

        if (attackAllowed)
        {
            attackTimer += 1f * Time.deltaTime;
            if (attackTimer >= settings.strikeDelayTime && Helpers.InRange(player.transform.position, Parent.transform.position, settings.strikeDistance))
            {
                attackAllowed = false;
                attackTimer = 0f;
                Attack();
            }
        }
        
        if (Vector3.Distance(player.transform.position, Parent.transform.position) > settings.loseDistance)
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
        Parent.rigidBody.velocity = Vector3.Normalize(walkDirections) * settings.attackWalkSpeed;
        Parent.animatorController.SetWalk((int)Mathf.Sign(-walkDirections.x), (int)Mathf.Sign(-walkDirections.y));
    }
    
    private void Attack()
    {
        Parent.animatorController.Attack();
        currentWalkSpeed = 1f;
        Parent.StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(.2f);
        Parent.attacking = true;
        currentWalkSpeed = settings.roamWalkSpeed;
        yield return new WaitForSeconds(.5f);
        Parent.attacking = false;
        attackAllowed = false;
    }
}