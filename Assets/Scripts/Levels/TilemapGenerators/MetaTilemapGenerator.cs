using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Levels.TilemapGenerators;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MetaTilemapGenerator:MonoBehaviour
{
    public Vector2Int tilemapSize;
    
    public BackgroundTilemapGenerator background;
    public CollisionTilemapGenerator collision;

    public DebugDrawBounds debugDraw;

    public int startSeed;

    private PathfindingController pathfinding;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(MetaTilemapGenerator));
    }

    private void Start()
    {
        pathfinding = WorldGraph.Retrieve(typeof(PathfindingController)) as PathfindingController;
    }

    public GenerateTilemapData Generate(Vector3Int root, GeneratorSet generatorSet)
    {
        Random.InitState(startSeed + root.y);
        var trimmedWidth = tilemapSize.x - 52;//trim value
        var trimmedHeight = tilemapSize.y - 8;//trim value
        
        //generate level plan bounds
        var collPosLeft = new Vector3Int(tilemapSize.x/4, tilemapSize.y/2, 0);
        var sourceBoundsLeft = new Bounds(collPosLeft, new Vector3Int(trimmedWidth/2, trimmedHeight, 0));
        var collPosRight = new Vector3Int(tilemapSize.x - tilemapSize.x/4, tilemapSize.y/2, 0);
        var sourceBoundsRight = new Bounds(collPosRight, new Vector3Int(trimmedWidth/2, trimmedHeight, 0));
        
        GenerateTilemapData generateData = new GenerateTilemapData(generatorSet); //!! is promise object and will be passed by reference

        var levelPlanLeft = BinarySpaceTree.Generate(sourceBoundsLeft, generateData);
        var levelPlanRight = BinarySpaceTree.Generate(sourceBoundsRight, generateData);
        
        generateData.planBounds = levelPlanLeft.Concat(levelPlanRight).ToArray();

        var toGenerateBG = background.Generate(ref generateData, tilemapSize);
        var toGenerateCollision = collision.Generate(ref generateData, tilemapSize, root);
        
        DrawPathsBetweenConstructs(generateData, ref toGenerateBG, generatorSet);
        
        generateData.background = ParseBlueprint(toGenerateBG, root);
        generateData.collision = ParseBlueprint(toGenerateCollision, root);
        
        debugDraw.SetBounds(generateData.planBounds, root);
        
        return generateData;
    }
    
    private void DrawPathsBetweenConstructs(GenerateTilemapData generateData, ref TileBase[,] bgTilemap, GeneratorSet generatorSet)
    {
        List<Tuple<Vector3Int,TileConstruct>> generatedWithPaths = generateData.generatedConstructs.FindAll(x => x.Item2.hasPaths);
        var ordered = generatedWithPaths.OrderBy(x => x.Item1.magnitude).ToList();
        
        for (int i = 0; i < ordered.Count-1; i++)
        {
            var construct = ordered[i];
            var nextConstruct = ordered[i+1];
            
            var deltaX = nextConstruct.Item1.x - construct.Item1.x;
            var deltaY = nextConstruct.Item1.y - construct.Item1.y;
            
            bgTilemap[construct.Item1.x, construct.Item1.y] = generatorSet.library.GetTile(TileLibraryKey.Path);

            if (deltaY < 0)
            {
                for (int j = 0; j >= deltaY; j--){ bgTilemap[construct.Item1.x, construct.Item1.y+j] = generatorSet.library.GetTile(TileLibraryKey.Path);}
            }
            else
            {
                for (int j = 0; j <= deltaY; j++) bgTilemap[construct.Item1.x, construct.Item1.y+j] = generatorSet.library.GetTile(TileLibraryKey.Path);
            }
            
            if (deltaX < 0)
            {
                for (int j = 0; j >= deltaX; j--) bgTilemap[construct.Item1.x+j, construct.Item1.y+deltaY] = generatorSet.library.GetTile(TileLibraryKey.Path);
            }
            else
            {
                for (int j = 0; j <= deltaX; j++) bgTilemap[construct.Item1.x+j, construct.Item1.y+deltaY] = generatorSet.library.GetTile(TileLibraryKey.Path);
            }
            
        }
    }
    

    private GenerateTilemapPair ParseBlueprint(TileBase[,] blueprint, Vector3Int offset)
    {
        GenerateTilemapPair result = new GenerateTilemapPair();

        for (int x = 0; x < blueprint.GetUpperBound(0); x++)
        {
            for (int y = 0; y < blueprint.GetUpperBound(1); y++)
            {
                result.tiles.Add(blueprint[x, y]);
                result.positions.Add(new Vector3Int(x+offset.x,y+offset.y,0));
            }
        }
        return result;
    }
  
}
