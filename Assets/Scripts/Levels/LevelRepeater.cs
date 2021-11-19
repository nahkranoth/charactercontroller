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
    
    private GenerateTilemapData tickBlueprint;

    private PlayerController player;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(LevelRepeater));
    }

    public void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        player.status.currentStep = 0;
        player.status.currentLowStep = 0;
        
        // player.status.OnUpdate -= Regenerate;
        // player.status.OnUpdate += Regenerate;
        Regenerate();
    }

    public void Regenerate()
    {
        GenerateAtRoot(player.status.currentStep);
        GenerateAtRoot(player.status.currentStep-StepSize());
        GenerateAtRoot(player.status.currentStep+StepSize());
        player.status.currentLowStep = player.status.currentStep-StepSize();
        player.status.currentStep = player.status.currentStep+StepSize();
    }

    private void GenerateAtRoot(int step)
    {
        var set = generatorCollection.GetByStep(Mathf.FloorToInt(step/StepSize()));
        tickBlueprint = metaTilemapGenerator.Generate(new Vector3Int(0,step,0), set);
        backgroundTilemap.SetTiles(tickBlueprint.GetBackgroundPositions(), tickBlueprint.GetBackgroundTiles());
        collisionTilemap.SetTiles(tickBlueprint.GetCollisionPositions(), tickBlueprint.GetCollisionTiles());
        OnGenerate?.Invoke();
        metaEntityPlacer.Generate(metaTilemapGenerator, new Vector3Int(0,step,0), generatorCollection.GetByStep(Mathf.FloorToInt(step/StepSize())));
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
        return player.status.currentStep + StepSize();
    }
    
    public int GetLowestGeneratePoint()
    {
        return player.status.currentLowStep;
    }

    private int StepSize()
    {
        return metaTilemapGenerator.tilemapSize.y - 1;
    }

    public void Increase()
    {
        var newStep = player.status.currentStep + StepSize();
        GenerateAtRoot(newStep);
        RemoveAt(player.status.currentLowStep-1);
        player.status.currentStep = newStep;
        player.status.currentLowStep += StepSize();
    }
    public void Decrease()
    {
        var newStep = player.status.currentLowStep - StepSize();
        GenerateAtRoot(newStep);
        RemoveAt(player.status.currentStep+1);
        player.status.currentStep -= StepSize();
        player.status.currentLowStep = newStep;
    }
   
}
