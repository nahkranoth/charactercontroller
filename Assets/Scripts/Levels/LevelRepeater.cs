using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelRepeater : MonoBehaviour
{
    public MetaTilemapGenerator metaTilemapGenerator;
    public MetaLevelEntityPlacer metaEntityPlacer;
    public Tilemap backgroundTilemap;
    public Tilemap collisionTilemap;
    
    private GenerateTilemapData tickBlueprint;
    private int highestStep;
    private int lowestStep;

    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(LevelRepeater));
    }

    public void OnInit()
    {
        lowestStep = 0;
        highestStep = 0;
        GenerateAtRoot(lowestStep);
    }

    private void GenerateAtRoot(int step)
    {
        tickBlueprint = metaTilemapGenerator.Generate(new Vector3Int(0,step,0));
        backgroundTilemap.SetTiles(tickBlueprint.GetBackgroundPositions(), tickBlueprint.GetBackgroundTiles());
        collisionTilemap.SetTiles(tickBlueprint.GetCollisionPositions(), tickBlueprint.GetCollisionTiles());

        RemoveAt();
    }

    public void RemoveAt()
    {
        List<Vector3Int> remove = new List<Vector3Int>();
        for (int x = 0; x < metaTilemapGenerator.tilemapSize.x; x++)
        {
            for (int y = 0; y < metaTilemapGenerator.tilemapSize.y; y++)
            {
                remove.Add(new Vector3Int(x, y, 0));
            } 
        }
        backgroundTilemap.SetTiles(remove.ToArray(), new TileBase[remove.Count]);
    }
    public int GetHighestGeneratePoint()
    {
        return highestStep;
    }
    
    public int GetLowestGeneratePoint()
    {
        return lowestStep;
    }
    
    public void Increase()
    {
        highestStep += metaTilemapGenerator.tilemapSize.y-1;
        GenerateAtRoot(highestStep);
        // metaEntityPlacer.Generate(lowestEntitySpawner, lowestTilemapGenerator);
    }
    public void Decrease()
    {
        Debug.Log("Decrease");
        lowestStep -= metaTilemapGenerator.tilemapSize.y-1;
        GenerateAtRoot(lowestStep);
        // metaEntityPlacer.Generate(highestEntitySpawner, highestTilemapGenerator);
    }
   
}
