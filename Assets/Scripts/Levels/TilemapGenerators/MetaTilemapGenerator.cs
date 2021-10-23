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

    public GeneratorSetCollection setCollection;
    
    public GenerateTilemapData Generate(Vector3Int root, int step)
    {
        Random.InitState(root.y);
        var trimmedWidth = tilemapSize.x - 52;//trim value
        var trimmedHeight = tilemapSize.y - 8;//trim value
        var set = setCollection.GetByStep(step);
        
        GenerateTilemapData generateData = new GenerateTilemapData(set);
        
        //generate level plan bounds
        var collPosLeft = new Vector3Int(tilemapSize.x/4, tilemapSize.y/2, 0);
        var sourceBoundsLeft = new Bounds(collPosLeft, new Vector3Int(trimmedWidth/2, trimmedHeight, 0));
        var collPosRight = new Vector3Int(tilemapSize.x - tilemapSize.x/4, tilemapSize.y/2, 0);
        var sourceBoundsRight = new Bounds(collPosRight, new Vector3Int(trimmedWidth/2, trimmedHeight, 0));
        
        var levelPlanLeft = BinarySpaceTree.Generate(sourceBoundsLeft, set.bitTreeSearchDepth);
        var levelPlanRight = BinarySpaceTree.Generate(sourceBoundsRight, set.bitTreeSearchDepth);
        
        generateData.planBounds = levelPlanLeft.Concat(levelPlanRight).ToArray();
        generateData.background = ParseBlueprint(background.Generate(generateData, tilemapSize), root);
        generateData.collision = ParseBlueprint(collision.Generate(generateData, tilemapSize, root), root);
        
        debugDraw.SetBounds(generateData.planBounds, root);
        
        return generateData;
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
