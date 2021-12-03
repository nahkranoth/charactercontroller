
using System.Collections.Generic;
using UnityEngine;

public static class RaritySelector
{
    public static IRandomProbability GetRandom(List<IRandomProbability> candidates)
    {
        var sum_of_weight = 0f;
        foreach (var candidate in candidates)
        {
            sum_of_weight += candidate.Probability;
        }
        return LookUp(candidates, Random.Range(0, sum_of_weight));
    }
    
    private static IRandomProbability LookUp(List<IRandomProbability> candidates, float random)
    {
        var cummul = 0f;
        foreach (var candidate in candidates)
        {
            cummul += candidate.Probability;
            if (random < cummul) return candidate;
        }

        return null;
    }
}
