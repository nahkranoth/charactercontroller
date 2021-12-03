
using System.Collections.Generic;
using UnityEngine;

public static class RaritySelector
{
    public static IRandomWeight GetRandom(List<IRandomWeight> candidates)
    {
        var sum_of_weight = 0f;
        foreach (var candidate in candidates)
        {
            sum_of_weight += candidate.Weight;
        }
        return LookUp(candidates, Random.Range(0, sum_of_weight));
    }
    
    private static IRandomWeight LookUp(List<IRandomWeight> candidates, float random)
    {
        var cummul = 0f;
        foreach (var candidate in candidates)
        {
            cummul += candidate.Weight;
            if (random < cummul) return candidate;
        }

        return null;
    }
}
