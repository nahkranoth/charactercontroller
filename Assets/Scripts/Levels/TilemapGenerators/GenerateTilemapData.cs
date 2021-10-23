using System.Collections.Generic;
using Levels.TilemapGenerators;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTilemapData
{
    public Bounds[] planBounds;

    public GenerateTilemapPair background;
    public GenerateTilemapPair collision;
    
    public TileLibrary library;
    public RuleTileLibrary ruleTiles;
    public TileConstructCollection constructCollection;

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


