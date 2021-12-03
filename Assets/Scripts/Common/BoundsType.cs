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
        var rarCandidates = probabilities.ToList<IRandomWeight>();
        var res = RaritySelector.GetRandom(rarCandidates) as BoundsTypeProbability;
        return res.type;
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
