using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelRepeater : MonoBehaviour
{
    public MetaTilemapGenerator metaTilemapGenerator;
    public MetaLevelEntityPlacer metaEntityPlacer;
    public Tilemap backgroundTilemap;
    public Tilemap collisionTilemap;
    public int keepLoaded;
    
    private GenerateTilemapData tickBlueprint;
    private int currentStep;

    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(LevelRepeater));
    }

    public void Start()
    {
        currentStep = 0;
        GenerateAtRoot(currentStep);
    }

    private void GenerateAtRoot(int step)
    {
        tickBlueprint = metaTilemapGenerator.Generate(new Vector3Int(0,step,0));
        backgroundTilemap.SetTiles(tickBlueprint.GetBackgroundPositions(), tickBlueprint.GetBackgroundTiles());
        collisionTilemap.SetTiles(tickBlueprint.GetCollisionPositions(), tickBlueprint.GetCollisionTiles());
        
        metaEntityPlacer.Generate(metaTilemapGenerator, new Vector3Int(0,step,0));
    }

    public void RemoveAt(int yPos)
    {
        List<Vector3Int> remove = new List<Vector3Int>();
        for (int x = 0; x < metaTilemapGenerator.tilemapSize.x; x++)
        {
            for (int y = 0; y < metaTilemapGenerator.tilemapSize.y; y++)
            {
                var pos = new Vector3Int(x, y + yPos, 0);
                metaEntityPlacer.RemoveAt(pos);
                remove.Add(pos);
            } 
        }

        var rmTileBase = new TileBase[remove.Count];
        backgroundTilemap.SetTiles(remove.ToArray(), rmTileBase);
        collisionTilemap.SetTiles(remove.ToArray(), rmTileBase);
    }
    public int GetHighestGeneratePoint()
    {
        return currentStep + StepSize();
    }
    
    public int GetLowestGeneratePoint()
    {
        return currentStep;
    }

    private int StepSize()
    {
        return metaTilemapGenerator.tilemapSize.y - 1;
    }
    
    public void Increase()
    {
        var newStep = currentStep + StepSize();
        GenerateAtRoot(newStep);
        RemoveAt(currentStep - StepSize() * keepLoaded);
        currentStep += StepSize();
        metaEntityPlacer.Generate(metaTilemapGenerator, new Vector3Int(0,newStep,0));
    }
    public void Decrease()
    {
        var newStep = currentStep - StepSize();
        GenerateAtRoot(newStep);
        RemoveAt(currentStep + StepSize() * keepLoaded);
        currentStep -= StepSize();
        // metaEntityPlacer.Generate(metaEntityPlacer.entityPlacer, metaTilemapGenerator, new Vector3Int(0,newStep,0));
    }
   
}
