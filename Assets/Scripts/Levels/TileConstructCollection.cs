using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "ConstructCollection", menuName = "Custom/ConstructCollection", order = 1)]
public class TileConstructCollection : ScriptableObject
{
    public List<TileConstruct> collection;

    public TileConstruct GetByBounds(Bounds bounds)
    {
        // int randomIndex = Random.Range(0, collection.Count);
        var candidate = collection.Find(x => x.size.x <= bounds.size.x && x.size.y <= bounds.size.y);
        return candidate;
    }
}
