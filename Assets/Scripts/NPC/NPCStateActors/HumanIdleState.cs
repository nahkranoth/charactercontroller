public class HumanIdleState:AbstractNPCState
{
    public override void Activate()
    {
        Parent.rigidBody.isKinematic = true;
    }

    public override void Deactivate()
    {
    }

    public override void Execute()
    {
    }
}
