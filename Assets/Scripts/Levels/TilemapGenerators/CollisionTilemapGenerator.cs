using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionTilemapGenerator : TilemapGenerator
{
    public TileConstructCollection buildings;
    private GenerateTilemapData data;
    public GameObject shadowSprite;
    public bool drawShadows = true;

    private List<TileConstruct> constructs = new List<TileConstruct>();//TODO would be cool to keep a track of all constructs (also needed for shadows)
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
            var construct = AddConstruct(buildings, constructPosition);
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
    
    public TileConstruct AddConstruct(TileConstructCollection constructs, Bounds bounds)
    {
        var construct = constructs.GetByBounds(bounds);
        if (construct == null) return null;

        for (var i = 0; i < construct.spray+1; i++)
        {
            var center = new Vector3Int((int) bounds.center.x, (int) bounds.center.y, 0);
            Vector3Int rPos = Helpers.RandomVector3((int) bounds.extents.x/2);
            var pos = construct.inCenterOfBounds ? center : center + rPos;
            DrawConstruct(construct, pos);
        }

        return construct;
    }

    private void DrawConstruct(TileConstruct construct, Vector3Int position)
    {
        var xSize = blueprint.GetUpperBound(0);
        var ySize = blueprint.GetUpperBound(1);

        Dictionary<Vector2Int, TileBase> tempBlueprint = new Dictionary<Vector2Int, TileBase>();
        
        foreach (var tile in construct.map)
        {
            var posX = position.x + tile.position.x - construct.size.x/2;
            var posY = position.y + tile.position.y - construct.size.y/2;
            if (posX >= 0 && posX <= xSize && posY >= 0 && posY <= ySize) //only pass if inside tilemap
            {
                if (blueprint[posX, posY] != null) return; //do not overwrite
                tempBlueprint[new Vector2Int(posX, posY)] = tile.tile;
            }
        }
        // I delay this to only overwrite when it doesnt hit anything else
        foreach (var tmp in tempBlueprint)
        {
            blueprint[tmp.Key.x, tmp.Key.y] = tmp.Value;
        }
        
    }

}