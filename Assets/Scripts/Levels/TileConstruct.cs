using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class TileConstruct:ScriptableObject, IRandomWeight
{
    public List<TileWrapper> map;
    public Vector2Int size;
    public BoundsType type;
    public float randomWeight;
    public Sprite constructSprite;
    public int spray;
    public bool inCenterOfBounds = false;
    public Vector3 shadowPositionOffset;
    public Vector3 shadowScaleOffset;

    public bool hasPaths;
    public bool destroyable;

    public List<EntitySpawner> entities = new List<EntitySpawner>();

    public EntityCollection dropOnDestroy;

    public TileBase ContainsTile(TileBase tile)
    {
        return map.Find(x => x.tile == tile)?.tile;
    }
    
    public float Weight
    {
        get { return randomWeight;}
        set { randomWeight = value; }
    }
}