using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GridController : MonoBehaviour
{
    public Grid grid;
    public MetaTilemapGenerator tickGenerator;
    public MetaTilemapGenerator tackGenerator;
    
    public void ColorTileAtCell(Vector3Int pos, Tilemap tilemap, Color color)
    {
        tilemap.SetTileFlags(pos, TileFlags.None);
        tilemap.SetColor(pos, color);
    }

}
