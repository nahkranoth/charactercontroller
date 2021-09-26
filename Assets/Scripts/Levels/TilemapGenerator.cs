using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase grass;
    public TileBase road;

    public Vector2Int size;
    public Action<Tilemap> OnDoneInit;
    
    private int[,] blueprint;
    private Dictionary<int, TileBase> tileBaseMap = new Dictionary<int, TileBase>();

    private void Start()
    {
        tileBaseMap.Add(1, grass);
        tileBaseMap.Add(2, road);
        Generate();
    }

    public void Generate()
    {
        blueprint = new int[size.x,size.y];
        InitBlueprint(ref blueprint);
        AddRoad(ref blueprint);
        BuildMap(blueprint);
    }

    private void BuildMap(int[,] map)
    {
        tilemap.ClearAllTiles();
        TileBase tb;
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                tileBaseMap.TryGetValue(map[x, y], out tb);
                tilemap.SetTile(new Vector3Int(x - size.x/2, y - size.y/2, 0), tb);
            }
        }
        OnDoneInit?.Invoke(tilemap);
    }

    private void InitBlueprint(ref int[,] map)
    {
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
                map[x, y] = 1;
    }
    
    private void AddRoad(ref int[,] map)
    {
        int currentX = size.x / 2;
        for (int y = 0; y < size.y; y++)
        {
            if(currentX > 0 && currentX < map.GetUpperBound(0)) map[currentX, y] = 2;
            if(currentX-1 > 0 && currentX-1 < map.GetUpperBound(0)) map[currentX-1, y] = 2;
            if(currentX+1 > 0 && currentX+1 < map.GetUpperBound(0)) map[currentX+1, y] = 2;
            currentX += Random.Range(-1, 2);
        }
    }
}
