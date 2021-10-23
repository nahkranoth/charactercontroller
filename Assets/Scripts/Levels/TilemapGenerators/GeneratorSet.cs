using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorSet", menuName = "Custom/GeneratorSet")]
public class GeneratorSet:ScriptableObject
{
    public int step = 0;
    public int bitTreeSearchDepth = 1;
    public int constructDensity = 20;
    public TileLibrary library;
    public RuleTileLibrary ruleTiles;
    public TileConstructCollection constructCollection;
}
