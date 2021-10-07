using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TilemapGenerator : MonoBehaviour
{
    public Vector2Int size;
    public Tilemap tilemap;
    public TileLibrary library;

    internal Dictionary<int, TileBase> tileBaseMap = new Dictionary<int, TileBase>();
    
    public Action<Tilemap> OnDoneInit;
    
    public void SetPosition(Vector3 pos)
    {
        tilemap.transform.localPosition = pos;
    }

    public float GetY()
    {
        return tilemap.transform.position.y;
    }
    
    internal void InitBlueprint(ref TileBase[,] map)
    {
        for (int x = 0; x < size.x; x++)
        for (int y = 0; y < size.y; y++)
            map[x, y] = library.GetTile(TileLibraryKey.Grass);
    }
    
    internal void BuildMap(TileBase[,] map)
    {
        tilemap.ClearAllTiles();
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                tilemap.SetTile(new Vector3Int(x - size.x/2, y - size.y/2, 0), map[x,y]);
            }
        }
        OnDoneInit?.Invoke(tilemap);
    }
}
