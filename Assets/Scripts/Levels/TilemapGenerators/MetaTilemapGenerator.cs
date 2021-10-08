using UnityEngine;

public class MetaTilemapGenerator:MonoBehaviour
{
    
    public Vector2Int tilemapSize;
    
    public BackgroundTilemapGenerator background;
    public CollisionTilemapGenerator collision;
    
    public void Start()
    {
        Generate();
    }

    public void Generate()
    {
        CollisionTilemapData collisionData = new CollisionTilemapData();
        BackgroundTilemapData backgroundData = new BackgroundTilemapData();

        background.Generate(backgroundData, tilemapSize);
        collision.Generate(collisionData, tilemapSize);
    }

    public float GetY()
    {
        return background.GetY();
    }
    
    public void SetPosition(Vector3 pos)
    {
        background.SetPosition(pos);
        collision.SetPosition(pos);
    }
}
