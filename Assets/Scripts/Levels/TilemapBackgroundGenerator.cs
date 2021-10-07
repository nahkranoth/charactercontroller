using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapBackgroundGenerator : TilemapGenerator
{
    public TileConstructCollection buildings;
    
    private TileBase[,] blueprint;

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        blueprint = new TileBase[size.x,size.y];
        InitBlueprint(ref blueprint);
        AddRoad(ref blueprint);
        AddConstruct(ref blueprint, buildings, new Vector3Int(12, 12, 0));
        BuildMap(blueprint);
    }

    public void AddConstruct(ref TileBase[,] map, TileConstructCollection constructs, Vector3Int offset)
    {
        int randomIndex = Random.Range(0, constructs.collection.Count);
        var construct = constructs.collection[0];
        
        foreach (var tile in construct.map)
        {
            map[tile.position.x, tile.position.y] = tile.tile;
        }
    }

    private void AddRoad(ref TileBase[,] map)
    {
        int xSize = map.GetUpperBound(0);
        int currentX = xSize / 2;
        int ySize = map.GetUpperBound(1);
        int halfY = ySize / 2;
        var road = library.GetTile(TileLibraryKey.Road);
        for (int y = 0; y < halfY+1; y++)
        {
            if(currentX > 0 && currentX < xSize) map[currentX, y] = road;
            if(currentX-1 > 0 && currentX-1 < xSize) map[currentX-1, y] = road;
            if(currentX+1 > 0 && currentX+1 < xSize) map[currentX+1, y] = road;
            //Mirror
            if(currentX > 0 && currentX < xSize) map[currentX, ySize-1-y] = road;
            if(currentX-1 > 0 && currentX-1 < xSize) map[currentX-1, ySize-1-y] = road;
            if(currentX+1 > 0 && currentX+1 < xSize) map[currentX+1, ySize-1-y] = road;
            
            currentX += Random.Range(-1, 2);
        }
    }
}
