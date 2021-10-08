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
        var collPos = new Vector3Int(tilemapSize.x/2, tilemapSize.y/2, 0);
        
        var sourceBounds = new Bounds(collPos, new Vector3Int(trimmedWidth/2, tilemapSize.y, 0));
        
        collisionData.constructBounds = BinarySpaceTree.Generate(sourceBounds, 4);
        backgroundData.floorPatternBounds = BinarySpaceTree.Generate(sourceBounds, 4);
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
