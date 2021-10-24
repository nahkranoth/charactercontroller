using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BoundsType
{
    House,
    Flora,
    Cliffs,
    CityScape,
    Tree
}

public static class BoundsTypeHelper
{
    public static BoundsType GetRandomBoundsType()
    {
        var values = Enum.GetValues(typeof(BoundsType));
        int i = Random.Range(0, values.Length);
        var result = (BoundsType)values.GetValue(i);
        return result;
    }

    public static Color GetDebugColor(BoundsType type)
    {
        switch (type)
        {
            case BoundsType.House:
                return Color.cyan;
            case BoundsType.Cliffs:
                return Color.magenta;
            case BoundsType.Flora:
                return Color.green;
            case BoundsType.Tree:
                return Color.yellow;
            case BoundsType.CityScape:
                return Color.gray;
            default:
                return Color.red;
        }
    }
}
