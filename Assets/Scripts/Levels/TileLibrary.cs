using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileLibrary", menuName = "Custom/TileLibrary", order = 1)]
public class TileLibrary : ScriptableObject
{
    public List<TileLibraryItem> library;

    public Tile GetTile(TileLibraryKey key)
    {
        return library.First(x => x.key == key).tile;
    }
    
    public TileLibraryKey GetKey(TileBase item)
    {
        return library.First(x => x.tile == item).key;
    }
}

[Serializable]
public struct TileLibraryItem
{
    public TileLibraryKey key;
    public Tile tile;
}

public enum TileLibraryKey
{
    Floor,
    Road,
    DimFloor,
    SolidFloor,
    Foliage,
    FloorFoliage,
    FloorFoliage2,
    FloorFoliage3,
    Path,
    Fence
}