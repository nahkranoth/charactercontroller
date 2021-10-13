using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileConstruct:ScriptableObject
{
    public List<TileWrapper> map;
    public Vector2Int size;
    public TileConstructType type;
    public int rarity = 100;
}

public enum TileConstructType
{
    House,
    Tree,
    Cliffs,
    CityScape
}