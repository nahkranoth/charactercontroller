using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GridController : MonoBehaviour
{
    public Grid grid;
    public Tilemap mainMap;
    public Tilemap collision;

    public Vector3Int GetRandomCellPosition(Tilemap tilemap)
    {
        var cellBounds = tilemap.cellBounds;
        int xR = Random.Range(cellBounds.min.x+1, cellBounds.max.x);
        int yR = Random.Range(cellBounds.min.y+1, cellBounds.max.y);
        return new Vector3Int(xR, yR, 0);
    }

    public bool HasCollisionOnCellPosition(Vector3Int pos)
    {
        var t = collision.GetTile(pos);
        return t != null;
    }
    
    public TileBase GetRandomTile(Tilemap tilemap)
    {
        return GetTileByCellPos(GetRandomCellPosition(tilemap), tilemap);
    }
    
    public TileBase GetTileByWorldPos(Vector3 pos, Tilemap tilemap)
    {
        var cellPos = grid.WorldToCell(pos);
        return GetTileByCellPos(cellPos, tilemap);
    }
    
    public TileBase GetTileByCellPos(Vector3Int pos, Tilemap tilemap)
    {
        return tilemap.GetTile(pos);
    }
    
    public void ColorTileAtCell(Vector3Int pos, Tilemap tilemap, Color color)
    {
        tilemap.SetTileFlags(pos, TileFlags.None);
        tilemap.SetColor(pos, color);
    }

}
