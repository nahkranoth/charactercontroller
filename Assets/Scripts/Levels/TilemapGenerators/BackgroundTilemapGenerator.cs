using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundTilemapGenerator : TilemapGenerator
{
    private TileBase[,] blueprint;

    private BackgroundTilemapData data;

    public void Generate(BackgroundTilemapData _data, Vector2Int mapSize)
    {
        size = mapSize;
        data = _data;
        blueprint = new TileBase[size.x,size.y];
        
        Fill(ref blueprint, TileLibraryKey.Floor);
        AddVerticalDrunk(ref blueprint, TileLibraryKey.Road, 0); //make road
        DrawBounds(ref blueprint, PullRandomGroup(data.floorPatternBounds, 4), TileLibraryKey.Foliage); //flower beds
        
        BuildMap(blueprint);
    }

    
}
