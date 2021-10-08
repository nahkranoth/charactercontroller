using UnityEngine;

public class MetaTilemapGenerator:MonoBehaviour
{
    
    public Vector2Int tilemapSize;
    
    public BackgroundTilemapGenerator background;
    public CollisionTilemapGenerator collision;

    public DebugDrawBounds debugDraw;
    
    public void Start()
    {
        Generate();
    }

    public void Generate()
    {
        CollisionTilemapData collisionData = new CollisionTilemapData();
        BackgroundTilemapData backgroundData = new BackgroundTilemapData();

        var trimmedWidth = tilemapSize.x - 52;//trim value
        var trimmedHeight = tilemapSize.y - 8;//trim value
        
        var collPosLeft = new Vector3Int(tilemapSize.x/4, tilemapSize.y/2, 0);
        var sourceBoundsLeft = new Bounds(collPosLeft, new Vector3Int(trimmedWidth/2, trimmedHeight, 0));
        
        var collPosRight = new Vector3Int(tilemapSize.x - tilemapSize.x/4, tilemapSize.y/2, 0);
        var sourceBoundsRight = new Bounds(collPosRight, new Vector3Int(trimmedWidth/2, trimmedHeight, 0));
        var levelPlan = BinarySpaceTree.Generate(new []{sourceBoundsRight, sourceBoundsLeft}, 4);
        
        collisionData.constructBounds = levelPlan;
        backgroundData.floorPatternBounds = levelPlan;
        debugDraw.SetBounds(backgroundData.floorPatternBounds, transform.localPosition);

        background.Generate(backgroundData, tilemapSize);
        collision.Generate(collisionData, tilemapSize);
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
