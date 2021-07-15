using UnityEngine;

public class CellData
{
    public Vector3 worldPos;
    public Vector3Int position;
    public CellData parent;
    public bool walkable;
    
    public float cost= 0;
    public float heuristics = 0;

    public float F
    {
        get { return cost + heuristics; }
    }
}
