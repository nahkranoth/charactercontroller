using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileConstruct:ScriptableObject, IRarity
{
    public List<TileWrapper> map;
    public Vector2Int size;
    public BoundsType type;
    public int rarity;
    public Sprite constructSprite;
    public int spray;
    public bool inCenterOfBounds = false;
    public Vector3 shadowPositionOffset;
    public Vector3 shadowScaleOffset;

    public bool hasPaths;

    public List<EntitySpawner> entities = new List<EntitySpawner>();
    
    public int Rarity
    {
        get { return rarity;}
        set { rarity = value; }
    }
}