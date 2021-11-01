using UnityEngine;

public class DonkeyIdleState:AbstractNPCState
{

    private float waitTime = 3f;
    private float currentTime;
    public override void Activate()
    {
        
    }

    public override void Execute()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= waitTime) Parent.SetState("follow");
    }
}