public class HumanIdleState:AbstractEnemyState
{
    public override void Activate()
    {
        Parent.rigidBody.isKinematic = true;
    }

    public override void Execute()
    {
    }
}
