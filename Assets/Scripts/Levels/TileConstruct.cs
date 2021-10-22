using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileConstruct:ScriptableObject, IRarity
{
    public List<TileWrapper> map;
    public Vector2Int size;
    public TileConstructType type;
    public int rarity;
    public Sprite constructSprite;

    public Vector3 shadowPositionOffset;
    public Vector3 shadowScaleOffset;
    
    public int Rarity
    {
        get { return rarity;}
        set { rarity = value; }
    }
}

public enum TileConstructType
{
    House,
    Flora,
    Cliffs,
    CityScape,
    Tree
}