using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileConstruct:ScriptableObject
{
    public string name;
    public List<TileWrapper> map;
    public Vector2Int size;
    public TileConstructType type;
}

public enum TileConstructType
{
    House,
    Tree,
    Cliffs
}