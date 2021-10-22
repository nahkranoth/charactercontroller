using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionTilemapGenerator : TilemapGenerator
{
    public TileConstructCollection buildings;
    private GenerateTilemapData data;
    public GameObject shadowSprite;
    public bool drawShadows = true;
    public TileBase[,] Generate(GenerateTilemapData _data, Vector2Int mapSize, Vector3Int root)
    {
        size = mapSize;
        data = _data;
        blueprint = new TileBase[size.x, size.y];

        //EAST WALL
        AddVerticalDrunkFillEast(TileLibraryKey.DimFloor, size.x/2-8); //make 
        Bounds[] eastwall = {new Bounds{center = new Vector3(size.x-4, 0, 0), size=new Vector3(4,size.y*2,0)}};
        FillBounds(eastwall, TileLibraryKey.DimFloor); //wall
        
        //WEST WALL
        Bounds[] westWall = {new Bounds{center = new Vector3(2, 0, 0), size=new Vector3(4,size.y*2,0)}};
        FillBounds(westWall, TileLibraryKey.SolidFloor); //wall
        
        //Constructs
        foreach (var constructPosition in PullRandomGroup(data.planBounds, 41))
        {
            var construct = AddConstruct(ref blueprint, buildings, constructPosition);
            if(construct != null && drawShadows) AddConstructShadowSprite(construct, constructPosition, root);
            if (construct?.type == TileConstructType.House)
            {
                DrawBoundsOutline(new []{constructPosition}, TileLibraryKey.Fence, .1f);
            }
        }
       
        return blueprint;
    }

    public void AddConstructShadowSprite(TileConstruct construct, Bounds position, Vector3Int root)
    {
        var shSprite = Instantiate(shadowSprite, transform, true).GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        shSprite.sprite = construct.constructSprite;
        var zOffset = new Vector3(0, 0, -shSprite.sprite.bounds.extents.y);
        var newPos = root + position.center + zOffset + construct.shadowPositionOffset;
        shSprite.transform.localPosition = newPos;
        shSprite.transform.localScale = shSprite.transform.localScale + construct.shadowScaleOffset;
    }
    
    public TileConstruct AddConstruct(ref TileBase[,] map, TileConstructCollection constructs, Bounds bounds)
    {
        var construct = constructs.GetByBounds(bounds);
        if (construct == null) return null;
        var xSize = map.GetUpperBound(0);
        var ySize = map.GetUpperBound(1);
        foreach (var tile in construct.map)
        {
            var posX = (int) bounds.center.x + tile.position.x - construct.size.x/2;
            var posY = (int) bounds.center.y + tile.position.y - construct.size.y/2;
            if(posX >= 0 && posX <= xSize && posY >= 0 && posY <= ySize) map[posX, posY] = tile.tile;
        }

        return construct;
    }

}