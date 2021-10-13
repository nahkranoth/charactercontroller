using System;
using System.Collections.Generic;
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
        candidates.ForEach(x => total += x.rarity);
        var randomI = Random.Range(0f, 1f);

        Debug.Log($"randomI: {randomI}");
        
        return GetRandom(candidates);
    }

    private TileConstruct GetRandom(List<TileConstruct> candidates)
    {
        var sum_of_weight = 0;
        foreach (var candidate in candidates)
        {
            sum_of_weight += candidate.rarity;
        }
        return LookUp(candidates, Random.Range(0, sum_of_weight));
    }
    
    
    private TileConstruct LookUp(List<TileConstruct> candidates, int random)
    {
        var cummul = 0;
        foreach (var candidate in candidates)
        {
            cummul += candidate.rarity;
            if (random < cummul) return candidate;
        }

        return null;
    }
}
