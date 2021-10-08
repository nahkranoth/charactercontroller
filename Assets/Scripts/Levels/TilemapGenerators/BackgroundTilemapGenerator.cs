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

        DrawBounds(data.floorPatternBounds, TileLibraryKey.SolidFloor);
        
        BuildMap(blueprint);
    }

    private void DrawBounds(Bounds[] bounds, TileLibraryKey tileKey)
    {
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
                    blueprint[centerX+x, centerY+y] = library.GetTile(tileKey);
                }
            }
        }
    }
}
