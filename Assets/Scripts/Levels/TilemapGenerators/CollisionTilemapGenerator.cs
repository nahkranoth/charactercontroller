using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionTilemapGenerator : TilemapGenerator
{
    public TileConstructCollection buildings;
    
    private TileBase[,] blueprint;

    private CollisionTilemapData data;

    public void Generate(CollisionTilemapData _data, Vector2Int mapSize)
    {
        size = mapSize;
        data = _data;
        blueprint = new TileBase[size.x, size.y];
        AddConstruct(ref blueprint, buildings, new Vector3Int(4, 4, 0));
        BuildMap(blueprint);
    }

    public void AddConstruct(ref TileBase[,] map, TileConstructCollection constructs, Vector3Int offset)
    {
        int randomIndex = Random.Range(0, constructs.collection.Count);
        var construct = constructs.collection[randomIndex];
        
        foreach (var tile in construct.map) map[offset.x + tile.position.x, offset.y + tile.position.y] = tile.tile;
    }
}