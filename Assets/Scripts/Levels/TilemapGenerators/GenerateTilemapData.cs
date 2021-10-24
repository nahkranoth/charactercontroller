using System.Collections.Generic;
using Levels.TilemapGenerators;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTilemapData
{
    public TypedBounds[] planBounds;

    public GenerateTilemapPair background;
    public GenerateTilemapPair collision;
    public TileConstructCollection constructCollection;
    public GeneratorSet set;
    
    public GenerateTilemapData(GeneratorSet _set)
    { 
        set = _set;
        constructCollection = set.constructCollection;
    }

    public Vector3Int[] GetBackgroundPositions()
    {
        return background.positions.ToArray();
    }
    public TileBase[] GetBackgroundTiles()
    {
        return background.tiles.ToArray();
    }

    public Vector3Int[] GetCollisionPositions()
    {
        return collision.positions.ToArray();
    }
    public TileBase[] GetCollisionTiles()
    {
        return collision.tiles.ToArray();
    }
    
}


