public abstract class AbstractNPCState
{
    private NPCController parent;

    public NPCController Parent
    {
        get { return parent; }
    }
    
    public void Init(NPCController _parent)
    {
        parent = _parent;
    }
    public abstract void Activate();
    public abstract void Execute();
}
