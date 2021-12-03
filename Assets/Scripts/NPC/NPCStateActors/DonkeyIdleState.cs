using UnityEngine;

public class DonkeyIdleState:AbstractNPCState
{

    private float waitTime = 3f;
    private float currentTime;
    public override void Activate()
    {
        currentTime = 0f;
        Parent.rigidBody.velocity = Vector3.zero;
        Parent.animatorController.SetWalk(0, 0);
    }

    public override void Execute()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= waitTime)
        {
            Parent.SetState("follow");
        }
    }
}