using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "ConstructCollection", menuName = "Custom/ConstructCollection", order = 1)]
public class TileConstructCollection : ScriptableObject
{
    public List<TileConstruct> collection;

    public TileConstruct GetByBounds(Bounds bounds)
    {
        var candidates = collection.FindAll(x => x.size.x <= bounds.size.x && x.size.y <= bounds.size.y);

        float total = 0f;
        candidates.ForEach(x => total += x.Rarity);
        var rarCandidates = candidates.ToList<IRarity>();
        return RaritySelector.GetRandom(rarCandidates) as TileConstruct;
    }
}
