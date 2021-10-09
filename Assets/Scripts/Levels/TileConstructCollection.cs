using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ConstructCollection", menuName = "Custom/ConstructCollection", order = 1)]
public class TileConstructCollection : ScriptableObject
{
    public List<TileConstruct> collection;

    public TileConstruct GetByBounds(Bounds bounds)
    {
        var candidates = collection.FindAll(x => x.size.x <= bounds.size.x && x.size.y <= bounds.size.y);
        if (candidates.Count == 0) return null;
        int randomIndex = Random.Range(0, candidates.Count);
        return candidates[randomIndex];
    }
}
