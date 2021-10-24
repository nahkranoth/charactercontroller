using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorSet", menuName = "Custom/GeneratorSet")]
public class GeneratorSet:ScriptableObject
{
    public int step = 0;
    public int bitTreeSearchDepth = 1;
    public int constructDensity = 20;
    public int enemyDensity = 4;
    public int containerDensity = 2;
    public bool hasRoad = true;
    public bool walledOff = false;
    public TileLibrary library;
    public RuleTileLibrary ruleTiles;
    public TileConstructCollection constructCollection;
    public List<BoundsTypeProbability> boundTypes;
}
