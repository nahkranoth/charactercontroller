using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundTilemapGenerator : TilemapGenerator
{
    private GenerateTilemapData data;

    public TileBase[,] Generate(GenerateTilemapData _data, Vector2Int mapSize, Vector3Int root)
    {
        size = mapSize;
        data = _data;
        blueprint = new TileBase[size.x,size.y];
        
        Fill(TileLibraryKey.Floor);
        DrawSpray(155, TileLibraryKey.FloorFoliage);
        DrawSpray(55, TileLibraryKey.FloorFoliage2);
        DrawSpray(12, TileLibraryKey.FloorFoliage3);
        
        AddVerticalDrunk(TileLibraryKey.Road, 0, 5); //make road

        var boundsGroup = PullRandomGroup(data.planBounds, 4);
        FillBounds(boundsGroup, TileLibraryKey.Foliage); //flower beds

        DrawBoundsOutline(boundsGroup, TileLibraryKey.Path, 0.05f);
        
        data.planBounds = data.planBounds.Except(boundsGroup).ToArray();
        
        return blueprint;
    }
}
