public class DonkeyDieState:AbstractNPCState
{
    private LevelEntityPlacer levelEntityPlacer;
    public override void Activate()
    {
        levelEntityPlacer = WorldGraph.Retrieve(typeof(LevelEntityPlacer)) as LevelEntityPlacer;
        levelEntityPlacer.RemoveTargetByEnemy(Parent);
        Parent.Destroy();
    }

    public override void Deactivate()
    {
    }

    public override void Execute()
    {
    
    }
}