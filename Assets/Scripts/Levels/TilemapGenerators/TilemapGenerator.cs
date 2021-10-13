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
    public TileBase[,] blueprint;

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
                if(WithinTilemap(offstCurrentX+x, xSize)) map[offstCurrentX+x, y] = road;
                //mirror
                if(WithinTilemap(offstCurrentX+x, xSize)) map[offstCurrentX+x, ySize-1-y] = road;
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
                if(WithinTilemap(offstCurrentX+x, xSize)) map[offstCurrentX+x, y] = road;
                //mirror
                if(WithinTilemap(offstCurrentX+x, xSize)) map[offstCurrentX+x, ySize-1-y] = road;
            }
          
            currentX += Random.Range(-1, 2);
        }
    }
    
    internal void FillBounds(ref TileBase[,] map, Bounds[] bounds, TileLibraryKey tileKey)
    {
        int xSize = map.GetUpperBound(0);
        int ySize = map.GetUpperBound(1);
        
        foreach (var patternBounds in bounds)
        {
            int centerX = (int) patternBounds.center.x;
            int centerY = (int) patternBounds.center.y;
            
            int extentX = (int) patternBounds.extents.x;
            int extentY = (int) patternBounds.extents.y;
            
            for (var x = -extentX; x < extentX+1; x++)
            {
                for (var y = -extentY; y < extentY+1; y++)
                {
                    var newX = centerX + x;
                    var newY = centerY + y;
                    if(WithinTilemap(newX, xSize, newY, ySize)) map[newX, newY] = library.GetTile(tileKey);
                }
            }
        }
    }
    
    internal void DrawBoundsOutline(ref TileBase[,] map, Bounds[] bounds, TileLibraryKey tileKey)
    {
        int xSize = map.GetUpperBound(0);
        int ySize = map.GetUpperBound(1);
        
        foreach (var patternBounds in bounds)
        {
            int centerX = (int) patternBounds.center.x;
            int centerY = (int) patternBounds.center.y;
            
            int extentX = (int) patternBounds.extents.x;
            int extentY = (int) patternBounds.extents.y;
            
            for (var x = -extentX; x < extentX + 1; x++)
            {
                var newX = centerX + x;
                if(WithinTilemap(newX,xSize, centerY+extentY, ySize)) map[newX, centerY+extentY] = library.GetTile(tileKey);
                if(WithinTilemap(newX,xSize, centerY-extentY, ySize)) map[newX, centerY-extentY] = library.GetTile(tileKey);
            }
            
            for (var y = -extentY; y < extentY; y++)
            {
                var newY = centerY + y;
                if(WithinTilemap(centerX+extentX-1,xSize, newY, ySize)) map[centerX+extentX, newY] = library.GetTile(tileKey);
                if(WithinTilemap(centerX+extentX,xSize, newY, ySize)) map[centerX-extentX, newY] = library.GetTile(tileKey);
            }
            
        }
    }

    private bool WithinTilemap(int val, int max)
    {
        return val >= 0 && val <= max;
    }
    
    private bool WithinTilemap(int valX, int maxX, int valY,  int maxY)
    {
        return valX >= 0 && valX <= maxX && valY >= 0 && valY <= maxY;
    }

    internal void DrawSpray(ref TileBase[,] map, int density, TileLibraryKey tileKey)
    {
        int xSize = map.GetUpperBound(0);
        int ySize = map.GetUpperBound(1);

        for (int i = 0; i < density; i++)
        {
            var xRange = Random.Range(0, xSize);
            var yRange = Random.Range(0, ySize);
            map[xRange, yRange] = library.GetTile(tileKey);
        }
        
    }

    public List<Vector3Int> GetAllTilesOfKey(TileLibraryKey key)
    {
        List<Vector3Int> result = new List<Vector3Int>();
        var search = library.GetTile(key);

        for (int x = 0; x < blueprint.GetUpperBound(0); x++)
        {
            for (int y = 0; y < blueprint.GetUpperBound(1); y++)
            {
                if (blueprint[x,y]?.name == search.name)
                {
                    result.Add(new Vector3Int(x,y,0));
                }
            }
        }
        return result;
    }

    public bool IsEmpty(Vector3 position)
    {
        return blueprint[(int)position.x, (int)position.y] == null;
    }
}
