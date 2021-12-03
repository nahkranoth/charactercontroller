using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BoundsType
{
    House,
    Flora,
    CityScape
}

public static class BoundsTypeHelper
{
    public static BoundsType GetRandomBoundsType(List<BoundsTypeProbability> probabilities)
    {
        Dictionary<int, float> probMap = new Dictionary<int, float>();
        for (int i = 0; i < probabilities.Count; i++)
        {
            probMap.Add(i, probabilities[i].probability);
        }
        var id = WeightedRandom.GetRandom(probMap);
        return probabilities[id].type;
    }

    public static Color GetDebugColor(BoundsType type)
    {
        switch (type)
        {
            case BoundsType.House:
                return Color.cyan;
            case BoundsType.Flora:
                return Color.green;
            case BoundsType.CityScape:
                return Color.gray;
            default:
                return Color.red;
        }
    }
}
