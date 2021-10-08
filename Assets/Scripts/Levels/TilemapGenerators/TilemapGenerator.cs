using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


public abstract class TilemapGenerator : MonoBehaviour
{
    [HideInInspector] public Vector2Int size;
    public Tilemap tilemap;
    public TileLibrary library;

    internal Dictionary<int, TileBase> tileBaseMap = new Dictionary<int, TileBase>();
    
    public Action<Tilemap> OnDoneInit;

    public Vector3 Position
    {
        get { return tilemap.transform.localPosition; }
        set{ tilemap.transform.localPosition = value; }
    }
    
    public float GetY()
    {
        return tilemap.transform.position.y;
    }
    
    internal void Fill(ref TileBase[,] map, TileLibraryKey key)
    {
        for (int x = 0; x < size.x; x++)
        for (int y = 0; y < size.y; y++)
            map[x, y] = library.GetTile(key);
    }
    
    internal void BuildMap(TileBase[,] map)
    {
        tilemap.ClearAllTiles();
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), map[x,y]);
            }
        }
        OnDoneInit?.Invoke(tilemap);
    }
    
    internal void AddVerticalDrunk(ref TileBase[,] map, TileLibraryKey tile, int offsetX)
    {
        int xSize = map.GetUpperBound(0);
        int currentX = xSize / 2;
        int ySize = map.GetUpperBound(1);
        int halfY = ySize / 2;
        var road = library.GetTile(tile);

        var offstCurrentX = 0;
        for (int y = 0; y < halfY+1; y++)
        {
            offstCurrentX = currentX + offsetX;
            if(offstCurrentX > 0 && offstCurrentX < xSize) map[offstCurrentX, y] = road;
            if(offstCurrentX-1 > 0 && offstCurrentX-1 < xSize) map[offstCurrentX-1, y] = road;
            if(offstCurrentX+1 > 0 && offstCurrentX+1 < xSize) map[offstCurrentX+1, y] = road;
            //Mirror
            if(offstCurrentX > 0 && offstCurrentX < xSize) map[offstCurrentX, ySize-1-y] = road;
            if(offstCurrentX-1 > 0 && offstCurrentX-1 < xSize) map[offstCurrentX-1, ySize-1-y] = road;
            if(offstCurrentX+1 > 0 && offstCurrentX+1 < xSize) map[offstCurrentX+1, ySize-1-y] = road;
            
            currentX += Random.Range(-1, 2);
        }
    }
}
