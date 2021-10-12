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
    public int boundsAreaSearchDepth;

    public GenerateTilemapData Generate(Vector3Int root)
    {
        Random.InitState(root.y);
        
        GenerateTilemapData generateData = new GenerateTilemapData();

        var trimmedWidth = tilemapSize.x - 52;//trim value
        var trimmedHeight = tilemapSize.y - 8;//trim value
        
        //generate level plan bounds
        var collPosLeft = new Vector3Int(tilemapSize.x/4, tilemapSize.y/2, 0);
        var sourceBoundsLeft = new Bounds(collPosLeft, new Vector3Int(trimmedWidth/2, trimmedHeight, 0));
        var collPosRight = new Vector3Int(tilemapSize.x - tilemapSize.x/4, tilemapSize.y/2, 0);
        var sourceBoundsRight = new Bounds(collPosRight, new Vector3Int(trimmedWidth/2, trimmedHeight, 0));
        
        var levelPlan = BinarySpaceTree.Generate(new []{sourceBoundsRight, sourceBoundsLeft}, boundsAreaSearchDepth);
        
        generateData.planBounds = levelPlan;
        generateData.background = ParseBlueprint(background.Generate(generateData, tilemapSize), root);
        generateData.collision = ParseBlueprint(collision.Generate(generateData, tilemapSize), root);
        
        // debugDraw.SetBounds(generateData.planBounds, background.Position * 16);
        
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
