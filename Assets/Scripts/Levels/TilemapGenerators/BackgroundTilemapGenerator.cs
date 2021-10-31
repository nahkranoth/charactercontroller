using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundTilemapGenerator : TilemapGenerator
{
    private GenerateTilemapData data;

    public TileBase[,] Generate(ref GenerateTilemapData _data, Vector2Int mapSize)
    {
        size = mapSize;
        data = _data;
        library = _data.set.library;
        ruleTiles = _data.set.ruleTiles;
        blueprint = new TileBase[size.x,size.y];
        
        Fill(TileLibraryKey.Floor);
        DrawSpray(155, TileLibraryKey.FloorFoliage);
        DrawSpray(55, TileLibraryKey.FloorFoliage2);
        DrawSpray(12, TileLibraryKey.FloorFoliage3);
        
        if(data.set.hasRoad) AddVerticalDrunk(RuleTileLibraryKey.Road, 0, 7); //make road

        var cropBounds = PullRandomGroup(data.planBounds, 4); //flower beds
        FillBounds(cropBounds, TileLibraryKey.Foliage);
        DrawBoundsOutline(cropBounds, TileLibraryKey.Path, 0.05f);
        data.planBounds = data.planBounds.Except(cropBounds).ToArray();// remove cropfields for the next generator

        return blueprint;
    }
}
