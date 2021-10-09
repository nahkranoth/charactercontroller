using System;
using System.Collections;
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
    
    internal Bounds[] PullRandomGroup(Bounds[] full, int amount)
    {
        Bounds[] result = new Bounds[amount];
        for (int i = 0; i < amount; i++)
        {
            var randID = Random.Range(0, full.Length);
            var candidate = full[randID];
            if (!((IList) result).Contains(candidate)) result[i] = candidate;
        }
        return result;
    }

    internal void AddVerticalDrunkFillEast(ref TileBase[,] map, TileLibraryKey tile, int offsetX)
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

            int edgeDiff = xSize - offstCurrentX;

            for (var x = 0; x < edgeDiff; x++)
            {
                if(offstCurrentX > 0 && offstCurrentX+x < xSize) map[offstCurrentX+x, y] = road;
                //mirror
                if(offstCurrentX > 0 && offstCurrentX+x < xSize) map[offstCurrentX+x, ySize-1-y] = road;
            }
          
            currentX += Random.Range(-1, 2);
        }
    }
    
    internal void AddVerticalDrunkFillWest(ref TileBase[,] map, TileLibraryKey tile, int offsetX)
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

            int edgeDiff = offstCurrentX;

            for (var x = 0; x < edgeDiff; x++)
            {
                if(offstCurrentX > 0 && offstCurrentX+x < xSize) map[offstCurrentX+x, y] = road;
                //mirror
                if(offstCurrentX > 0 && offstCurrentX+x < xSize) map[offstCurrentX+x, ySize-1-y] = road;
            }
          
            currentX += Random.Range(-1, 2);
        }
    }
    
    internal void AddVerticalDrunk(ref TileBase[,] map, TileLibraryKey tile, int offsetX, int thickness=3)
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

            for (int x = -thickness/2; x < thickness/2; x++)
            {
                if(offstCurrentX > 0 && offstCurrentX+x < xSize) map[offstCurrentX+x, y] = road;
                //mirror
                if(offstCurrentX > 0 && offstCurrentX+x < xSize) map[offstCurrentX+x, ySize-1-y] = road;
            }
          
            currentX += Random.Range(-1, 2);
        }
    }
    
    internal void DrawBounds(ref TileBase[,] map, Bounds[] bounds, TileLibraryKey tileKey)
    {
        int xSize = map.GetUpperBound(0);
        int ySize = map.GetUpperBound(1);
        
        foreach (var patternBounds in bounds)
        {
            int centerX = (int) patternBounds.center.x;
            int centerY = (int) patternBounds.center.y;
            
            int extentX = (int) patternBounds.extents.x;
            int extentY = (int) patternBounds.extents.y;
            
            for (var x = -extentX-1; x < extentX+1; x++)
            {
                for (var y = -extentY; y < extentY+1; y++)
                {
                    var newX = centerX + x;
                    var newY = centerY + y;
                    if(newX >= 0 && newX <= xSize && newY >= 0 && newY <= ySize) map[newX, newY] = library.GetTile(tileKey);
                }
            }
        }
    }
}
