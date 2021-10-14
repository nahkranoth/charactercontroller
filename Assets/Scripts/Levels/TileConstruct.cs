using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileConstruct:ScriptableObject, IRarity
{
    public List<TileWrapper> map;
    public Vector2Int size;
    public TileConstructType type;
    public int Rarity
    {
        get;
        set;
    }
}

public enum TileConstructType
{
    House,
    Tree,
    Cliffs,
    CityScape
}