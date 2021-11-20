using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class LevelRepeater : MonoBehaviour
{
    public MetaTilemapGenerator metaTilemapGenerator;
    public MetaLevelEntityPlacer metaEntityPlacer;
    public Tilemap backgroundTilemap;
    public Tilemap collisionTilemap;

    public Action OnGenerate;
    public GeneratorSetCollection generatorCollection;
    public int startSeed;

    private GenerateTilemapData tickBlueprint;

    private PlayerController player;
    private SaveLoad saveLoad;
    
    

    private List<int> activeSteps = new List<int>();
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(LevelRepeater));
    }

    public void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;

        player.statusController.OnFullReset -= Regenerate;
        player.statusController.OnFullReset += Regenerate;

        player.statusController.status.currentStep = 0;
        player.statusController.status.currentLowStep = 0;
        
        GenerateAtRoot(player.statusController.status.currentStep);
        GenerateAtRoot(player.statusController.status.currentStep-StepSize());
        GenerateAtRoot(player.statusController.status.currentStep+StepSize());
        
        player.statusController.status.currentLowStep = player.statusController.status.currentStep-StepSize();
        player.statusController.status.currentStep = player.statusController.status.currentStep+StepSize();
    }

    public void Regenerate()
    {
        RemoveAll();
        GenerateAtRoot(player.statusController.status.currentStep);
        GenerateAtRoot(player.statusController.status.currentStep-StepSize());
        GenerateAtRoot(player.statusController.status.currentStep+StepSize());
        
        player.statusController.status.currentLowStep = player.statusController.status.currentStep-StepSize();
        player.statusController.status.currentStep = player.statusController.status.currentStep+StepSize();
    }

    private void GenerateAtRoot(int step)
    {
        Random.InitState(startSeed + step);
        activeSteps.Add(step);
        var set = generatorCollection.GetByStep(Mathf.FloorToInt(step/StepSize()));
        tickBlueprint = metaTilemapGenerator.Generate(new Vector3Int(0,step,0), set);
        backgroundTilemap.SetTiles(tickBlueprint.GetBackgroundPositions(), tickBlueprint.GetBackgroundTiles());
        collisionTilemap.SetTiles(tickBlueprint.GetCollisionPositions(), tickBlueprint.GetCollisionTiles());
        OnGenerate?.Invoke();
        metaEntityPlacer.Generate(metaTilemapGenerator, new Vector3Int(0,step,0), set);
    }

    private void RemoveAll()
    {
        List<int> tempToRemove = new List<int>();
        foreach (var mstep in activeSteps) tempToRemove.Add(mstep);
        foreach (var tRemove in tempToRemove) RemoveAt(tRemove);
    }

    public void RemoveAt(int step)
    {
        activeSteps.Remove(step);
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
        return player.statusController.status.currentStep + StepSize();
    }
    
    public int GetLowestGeneratePoint()
    {
        return player.statusController.status.currentLowStep;
    }

    private int StepSize()
    {
        return metaTilemapGenerator.tilemapSize.y - 1;
    }

    public void Increase()
    {
        var newStep = player.statusController.status.currentStep + StepSize();
        GenerateAtRoot(newStep);
        RemoveAt(player.statusController.status.currentLowStep-1);
        player.statusController.status.currentStep = newStep;
        player.statusController.status.currentLowStep += StepSize();
    }
    public void Decrease()
    {
        var newStep = player.statusController.status.currentLowStep - StepSize();
        GenerateAtRoot(newStep);
        RemoveAt(player.statusController.status.currentStep+1);
        player.statusController.status.currentStep -= StepSize();
        player.statusController.status.currentLowStep = newStep;
    }
   
}
