public abstract class AbstractEnemyState
{
    private EnemyController parent;

    public EnemyController Parent
    {
        get { return parent; }
    }
    
    public void Init(EnemyController _parent)
    {
        parent = _parent;
    }
    public abstract void Activate();
    public abstract void Execute();
}
