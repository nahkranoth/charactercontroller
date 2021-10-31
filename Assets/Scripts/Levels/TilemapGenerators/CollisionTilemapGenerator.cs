using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionTilemapGenerator : TilemapGenerator
{
    private TileConstructCollection constructs;
    private GenerateTilemapData data;
    public GameObject shadowSprite;
    public bool drawShadows = true;


    private MetaLevelEntityPlacer metaEntityPlacer;

    private void Awake()
    {
        metaEntityPlacer = WorldGraph.Retrieve(typeof(MetaLevelEntityPlacer)) as MetaLevelEntityPlacer;
    }

    //private List<TileConstruct> constructs = new List<TileConstruct>();//TODO would be cool to keep a track of all constructs (also needed for shadows)
    public TileBase[,] Generate(ref GenerateTilemapData _data, Vector2Int mapSize, Vector3Int root)
    {
        size = mapSize;
        data = _data;
        library = _data.set.library;
        ruleTiles = _data.set.ruleTiles;
        constructs = _data.constructCollection;
        blueprint = new TileBase[size.x, size.y];

        //EAST WALL
        AddVerticalDrunkFillEast(TileLibraryKey.DimFloor, size.x/2-8); //make 
        TypedBounds[] eastwall = { new TypedBounds { bounds = new Bounds{center = new Vector3(size.x-4, 0, 0), size=new Vector3(4,size.y*2,0)}}};
        FillBounds(eastwall, TileLibraryKey.DimFloor); //wall
        
        //WEST WALL
        TypedBounds[] westWall = {new TypedBounds{bounds=new Bounds{center = new Vector3(2, 0, 0), size=new Vector3(4,size.y*2,0)}}};
        FillBounds(westWall, TileLibraryKey.SolidFloor); //wall

        if (data.set.walledOff)
        {
            //SOUTH WALL
            TypedBounds[] southWall = {new TypedBounds{bounds=new Bounds{center = new Vector3(0, 2, 0), size=new Vector3(size.x*2,4,0)}}};
            FillBounds(southWall, RuleTileLibraryKey.CityWall); //wall
            TypedBounds[] northWallOne = {new TypedBounds{bounds=new Bounds{center = new Vector3(0, size.y-4, 0), size=new Vector3(size.x-16,4,0)}}};
            FillBounds(northWallOne, RuleTileLibraryKey.CityWall); //wall
            TypedBounds[] northWallTwo = {new TypedBounds{bounds=new Bounds{center = new Vector3(size.x, size.y-4, 0), size=new Vector3(size.x-16,4,0)}}};
            FillBounds(northWallTwo, RuleTileLibraryKey.CityWall); //wall
        }
        
        //Constructs
        var group = PullRandomGroup(data.planBounds, data.set.constructDensity);
        foreach (var tBounds in group)
        {
            var construct = AddConstruct(constructs, tBounds);
            if(construct != null && drawShadows) AddConstructShadowSprite(construct, tBounds.bounds, root);
            if (construct?.type == BoundsType.House)
            {
                DrawBoundsOutline(new []{tBounds}, TileLibraryKey.Fence, .1f);
            }

            
            //Spawn entities connected to the construct
            if (construct == null) continue;
            foreach (var eSpawn in construct.entities)
            {
                var pos= Vector3Int.RoundToInt(tBounds.bounds.center + eSpawn.position);
                if (eSpawn.npcSpawner)
                {
                    metaEntityPlacer.entityPlacer.GenerateNPC(eSpawn.entity, pos);
                    continue;
                }
                if(eSpawn.collectableSpawner) metaEntityPlacer.entityPlacer.GenerateCollectable(eSpawn.entity, pos);
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
    
    public TileConstruct AddConstruct(TileConstructCollection constructs, TypedBounds tBounds)
    {
        var construct = constructs.GetByBounds(tBounds);
        if (construct == null) return null;

        for (var i = 0; i < construct.spray+1; i++)
        {
            var center = new Vector3Int((int) tBounds.bounds.center.x, (int) tBounds.bounds.center.y, 0);
            Vector3Int rPos = Helpers.RandomVector3((int) tBounds.bounds.extents.x/2);
            var pos = construct.inCenterOfBounds ? center : center + rPos;
            DrawConstruct(construct, pos);
        }

        return construct;
    }

    private void DrawConstruct(TileConstruct construct, Vector3Int position)
    {
        var xSize = blueprint.GetUpperBound(0);
        var ySize = blueprint.GetUpperBound(1);
        
        data.generatedConstructs.Add(new Tuple<Vector3Int, TileConstruct>(position, construct));
        
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