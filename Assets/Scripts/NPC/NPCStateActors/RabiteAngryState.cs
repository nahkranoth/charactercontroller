using System.Collections;
using UnityEngine;

public class RabiteAngryState: AbstractEnemyState
{
    private PlayerController player;
    private float currentWalkSpeed = 0.1f;
    private bool attacking;
    private RabiteSettings settings;
    private Vector3 roamTarget;
    private Vector3 walkDirections;

    private bool refindPathAllowed = true;
    public RabiteAngryState(RabiteSettings _settings)
    {
        settings = _settings;
    }
    
    public override void Activate()
    {
        InitializePath();
    }

    private void InitializePath()
    {
        var newPath = Parent.pathfinding.FindPathToPlayerByWorldPos(Parent.transform.position, 4);
        Parent.npcPathController.InitializePath(newPath);
        Parent.npcPathController.NextNode();
        roamTarget = Parent.npcPathController.GetTarget();
    }

    public override void Execute()
    {
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
        Parent.rigidBody.velocity = Vector3.Normalize(walkDirections) * settings.attackSpeed;
        Parent.animatorController.SetWalk((int)Mathf.Sign(-walkDirections.x), (int)Mathf.Sign(-walkDirections.y));
    }
    
    private void WithinStrikingDistance()
    {
        var val = Random.value;
        if (!attacking && val >= settings.strikingChance)
        {
            attacking = true;
            Parent.attacking = true;
            Attack();
        }
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
        currentWalkSpeed = settings.attackSpeed;
        yield return new WaitForSeconds(4f);
        attacking = false;
    }
}