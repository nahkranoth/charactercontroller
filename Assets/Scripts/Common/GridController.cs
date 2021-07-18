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
    public bool HasCollisionOnCellPosition(Vector3Int pos)
    {
        var t = collision.GetTile(pos);
        return t != null;
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
