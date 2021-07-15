using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class ZombieRoamState: AbstractEnemyState
{
    public float maxRoamDistance = 2f;
    public float walkSpeed = 0.01f;
    private float roamChance = 0.1f;
    private bool moving = false;
    private Vector3 roamTarget;
    private PlayerController player;
    private RandomController random;

    public ZombieRoamState(float _maxRoamDistance, float _walkSpeed, float _roamChance)
    {
        maxRoamDistance = _maxRoamDistance;
        walkSpeed = _walkSpeed;
        roamChance = _roamChance;
        random = WorldGraph.Retrieve(typeof(RandomController)) as RandomController;
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
    }

    public override void Activate()
    {
        moving = false;
        Parent.rigidBody.velocity = Vector2.zero;
        roamTarget = Parent.npcPathController.GetTarget();
    }

    public override void Execute()
    {
        if (Vector3.Distance(player.transform.position, Parent.transform.position) < 1f)
        {
            Parent.SetState("angry");
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
                Parent.SetState("idle");
            }
        }
        SetVelocity();
    }

    private void SetVelocity()
    {
        var walkDirections = Parent.npcPathController.FindDeltaVecOfCurrentNode(Parent.transform.position);
        Parent.rigidBody.velocity = Vector3.Normalize(walkDirections) * walkSpeed;
        Parent.animatorController.SetWalk((int)Mathf.Sign(-walkDirections.x), (int)Mathf.Sign(-walkDirections.y));
    }
    
}
