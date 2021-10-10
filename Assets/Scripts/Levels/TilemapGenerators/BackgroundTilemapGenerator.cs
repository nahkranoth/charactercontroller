using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundTilemapGenerator : TilemapGenerator
{
    private GenerateTilemapData data;

    public void Generate(GenerateTilemapData _data, Vector2Int mapSize)
    {
        size = mapSize;
        data = _data;
        blueprint = new TileBase[size.x,size.y];
        
        Fill(ref blueprint, TileLibraryKey.Floor);
        DrawSpray(ref blueprint, 155, TileLibraryKey.FloorFoliage);
        DrawSpray(ref blueprint, 55, TileLibraryKey.FloorFoliage2);
        DrawSpray(ref blueprint, 12, TileLibraryKey.FloorFoliage3);
        
        AddVerticalDrunk(ref blueprint, TileLibraryKey.Path, 0, 5); //make road
        
        var boundsGroup = PullRandomGroup(data.planBounds, 4);
        FillBounds(ref blueprint, boundsGroup, TileLibraryKey.Foliage); //flower beds

        DrawBoundsOutline(ref blueprint, boundsGroup, TileLibraryKey.Road);
        
        data.planBounds = data.planBounds.Except(boundsGroup).ToArray();
        
        BuildMap(blueprint);
    }
    
    
}
