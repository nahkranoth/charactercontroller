using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorSet", menuName = "Custom/GeneratorSet")]
public class GeneratorSet:ScriptableObject
{
    public int step = 0;
    public TileLibrary library;
    public RuleTileLibrary ruleTiles;
    public TileConstructCollection constructCollection;
}
