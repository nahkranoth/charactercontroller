using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase grass;
    public TileBase road;

    public Vector2Int size;
    
    private int[,] map;
    private void Start()
    {
        map = new int[size.x,size.y];
        InitMap(ref map);
        BuildMap(map);
    }

    private void BuildMap(int[,] map)
    {
        tilemap.ClearAllTiles();
        for (int x = 0; x < map.GetUpperBound(0); x++)
            for (int y = 0; y < map.GetUpperBound(1); y++)
                tilemap.SetTile(new Vector3Int(x - size.x/2, y - size.y/2, 0), grass);
    }

    private void InitMap(ref int[,] map)
    {
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
                map[x, y] = 1;
    }
}
