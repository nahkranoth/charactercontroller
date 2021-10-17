using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

public class LevelRepeater : MonoBehaviour
{
    public MetaTilemapGenerator metaTilemapGenerator;
    public MetaLevelEntityPlacer metaEntityPlacer;
    public Tilemap backgroundTilemap;
    public Tilemap collisionTilemap;
    public int keepLoaded;

    public Action OnGenerate;
    
    private GenerateTilemapData tickBlueprint;
    private int currentStep;
    private int currentLowStep;

    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(LevelRepeater));
    }

    public void Start()
    {
        currentStep = 0;
        currentLowStep = 0;
        GenerateAtRoot(currentStep);
        GenerateAtRoot(currentStep-StepSize());
        GenerateAtRoot(currentStep+StepSize());
        currentLowStep = currentStep-StepSize();
        currentStep = currentStep+StepSize();
    }

    private void GenerateAtRoot(int step)
    {
        tickBlueprint = metaTilemapGenerator.Generate(new Vector3Int(0,step,0));
        backgroundTilemap.SetTiles(tickBlueprint.GetBackgroundPositions(), tickBlueprint.GetBackgroundTiles());
        collisionTilemap.SetTiles(tickBlueprint.GetCollisionPositions(), tickBlueprint.GetCollisionTiles());
        OnGenerate?.Invoke();
        metaEntityPlacer.Generate(metaTilemapGenerator, new Vector3Int(0,step,0));
    }

    public void RemoveAt(int step)
    {
        List<Vector3Int> remove = new List<Vector3Int>();
        Vector3Int pos = Vector3Int.zero;
        for (int x = 0; x < metaTilemapGenerator.tilemapSize.x; x++)
        {
            for (int y = 0; y < metaTilemapGenerator.tilemapSize.y; y++)
            {
                pos = new Vector3Int(x, y + step, 0);
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
        return currentLowStep;
    }

    private int StepSize()
    {
        return metaTilemapGenerator.tilemapSize.y - 1;
    }

    //TODO Not Perfect, regenerates unnecesary
    
    public void Increase()
    {
        var newStep = currentStep + StepSize();
        GenerateAtRoot(newStep);
        RemoveAt(currentLowStep-1);
        currentStep = newStep;
        currentLowStep += StepSize();
        metaEntityPlacer.Generate(metaTilemapGenerator, new Vector3Int(0,newStep,0));
    }
    public void Decrease()
    {
        var newStep = currentLowStep - StepSize();
        GenerateAtRoot(newStep);
        RemoveAt(currentStep+1);
        currentStep -= StepSize();
        currentLowStep = newStep;
        metaEntityPlacer.Generate(metaTilemapGenerator, new Vector3Int(0,newStep,0));
    }
   
}
