using System.Collections;
using UnityEngine;

public class DonkeyFleeState: AbstractNPCState
{
    private PlayerController player;
    private float currentWalkSpeed = 0.1f;
    private float calmTimer = 10f;
    private INPCSettings settings;
    private Vector3 walkDirections;

    public DonkeyFleeState(INPCSettings _settings)
    {
        settings = _settings;
    }
    
    public override void Activate()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        
    }
    
    public override void Execute()
    {
        
        SetVelocity();
    }
    
    private void SetVelocity()
    {
        var walkDirections = Parent.npcPathController.FindDeltaVecOfCurrentNode(Parent.transform.position);
        Parent.rigidBody.velocity = Vector3.Normalize(walkDirections) * settings.roamWalkSpeed;
        
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