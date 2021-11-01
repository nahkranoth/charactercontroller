public class HumanIdleState:AbstractNPCState
{
    public override void Activate()
    {
        Parent.rigidBody.isKinematic = true;
    }

    public override void Execute()
    {
    }
}
