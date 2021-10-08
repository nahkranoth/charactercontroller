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
        
        foreach (var patternBounds in data.floorPatternBounds)
        {
            int mX = (int) patternBounds.center.x;
            int mY = (int) patternBounds.center.y;
            blueprint[mX, mY] = library.GetTile(TileLibraryKey.SolidFloor);
        }
        
        BuildMap(blueprint);

        
    }
}
