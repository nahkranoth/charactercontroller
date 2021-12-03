using System.Collections.Generic;
using UnityEngine;

public static class WeightedRandom
{
    public static int GetRandom(Dictionary<int, float> idToProbability)
    {
        var fullWeight = 0f;
        int selectedEntity = 0;
        foreach (var probability in idToProbability)
        {
            fullWeight += probability.Value;
        }

        var iR = Random.Range(0, fullWeight);
        
        foreach (var rc in idToProbability)
        {
            if (iR < rc.Value)
            {
                selectedEntity = rc.Key;
                break;
            }

            iR = iR - rc.Value;
        }
        return selectedEntity;
    }
}
