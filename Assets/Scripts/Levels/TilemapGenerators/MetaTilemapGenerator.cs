using UnityEngine;

public class MetaTilemapGenerator:MonoBehaviour
{
    
    public Vector2Int tilemapSize;
    
    public BackgroundTilemapGenerator background;
    public CollisionTilemapGenerator collision;

    public DebugDrawBounds debugDraw;
    public int boundsAreaSearchDepth;

    public void Generate()
    {
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
        background.Generate(generateData, tilemapSize);
        collision.Generate(generateData, tilemapSize);
        debugDraw.SetBounds(generateData.planBounds, background.Position * 16);
    }

    public float GetY()
    {
        return background.GetY();
    }
    
    public void SetPosition(Vector3 pos)
    {
        background.Position = pos;
        collision.Position = pos;
    }
}
