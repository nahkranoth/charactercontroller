
using System.Collections.Generic;
using UnityEngine;

public static class RaritySelector
{
    public static IRarity GetRandom(List<IRarity> candidates)
    {
        var sum_of_weight = 0;
        foreach (var candidate in candidates)
        {
            sum_of_weight += candidate.Rarity;
        }
        return LookUp(candidates, Random.Range(0, sum_of_weight));
    }
    
    
    private static IRarity LookUp(List<IRarity> candidates, int random)
    {
        var cummul = 0;
        foreach (var candidate in candidates)
        {
            cummul += candidate.Rarity;
            if (random < cummul) return candidate;
        }

        return null;
    }
}
