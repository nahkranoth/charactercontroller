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

        //EAST WALL
        AddVerticalDrunkFillEast(ref blueprint, TileLibraryKey.DimFloor, size.x/2-8); //make 
        Bounds[] eastwall = {new Bounds{center = new Vector3(size.x-4, 0, 0), size=new Vector3(4,size.y*2,0)}};
        DrawBounds(ref blueprint, eastwall, TileLibraryKey.DimFloor); //wall
        
        //WEST WALL
        Bounds[] westWall = {new Bounds{center = new Vector3(3, 0, 0), size=new Vector3(4,size.y*2,0)}};
        DrawBounds(ref blueprint, westWall, TileLibraryKey.SolidFloor); //wall
        
        //Constructs
        foreach (var constructPosition in PullRandomGroup(data.constructBounds, 14))
        {
            AddConstruct(ref blueprint, buildings, constructPosition);
        }
        
        BuildMap(blueprint);
    }

    public void AddConstruct(ref TileBase[,] map, TileConstructCollection constructs, Bounds bounds)
    {
        var construct = constructs.GetByBounds(bounds);
        if (construct == null) return;
        var xSize = map.GetUpperBound(0);
        var ySize = map.GetUpperBound(1);
        foreach (var tile in construct.map)
        {
            var posX = (int) bounds.center.x + tile.position.x - construct.size.x/2;
            var posY = (int) bounds.center.y + tile.position.y - construct.size.y/2;
            if(posX >= 0 && posX <= xSize && posY >= 0 && posY <= ySize) map[posX, posY] = tile.tile;
        }
    }
}