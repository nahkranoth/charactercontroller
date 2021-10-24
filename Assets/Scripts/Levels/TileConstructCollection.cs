using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ConstructCollection", menuName = "Custom/ConstructCollection", order = 1)]
public class TileConstructCollection : ScriptableObject
{
    public List<TileConstruct> collection;

    public TileConstruct GetByBounds(TypedBounds tBounds)
    {
        var candidates = collection.FindAll(
            x => 
                x.size.x <= tBounds.bounds.size.x && 
                x.size.y <= tBounds.bounds.size.y && 
                x.type == tBounds.type
                );
        
        float total = 0f;
        candidates.ForEach(x => total += x.Rarity);
        var rarCandidates = candidates.ToList<IRarity>();
        return RaritySelector.GetRandom(rarCandidates) as TileConstruct;
    }
}
