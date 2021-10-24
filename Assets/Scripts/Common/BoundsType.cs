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
        var fullWeight = 0;
        foreach (var prob in probabilities)
        {
            fullWeight += prob.probability;
        }

        int iR = Random.Range(0, fullWeight);

        BoundsTypeProbability selectedBoundsType = null;
        foreach (BoundsTypeProbability bt in probabilities)
        {
            if (iR < bt.probability)
            {
                selectedBoundsType = bt;
                break;
            }

            iR = iR - bt.probability;
        }
        return selectedBoundsType.type;
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
