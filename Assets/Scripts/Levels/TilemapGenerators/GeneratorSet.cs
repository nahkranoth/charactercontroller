using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorSet", menuName = "Custom/GeneratorSet")]
public class GeneratorSet:ScriptableObject, IRandomProbability
{
    public int bitTreeSearchDepth = 1;
    public int constructDensity = 20;
    public int npcDensity = 4;
    public int containerDensity = 2;
    public bool hasRoad = true;
    public float spawnProbability = 1f;
    public WallType northWall = WallType.None;
    public WallType southWall = WallType.None;

    [Tooltip("If this set is chosen by the GeneratorSetCollection it wont be repeated")]
    public bool onlyEverOneInGenerator = false;
    
    public TileLibrary library;
    public RuleTileLibrary ruleTiles;
    public TileConstructCollection constructCollection;
    public List<BoundsTypeProbability> boundTypes;

    public EntityCollection containers;
    public EntityCollection npcs;
    public float Probability => spawnProbability;
}

public enum WallType{
    Gate,
    Full,
    None
}