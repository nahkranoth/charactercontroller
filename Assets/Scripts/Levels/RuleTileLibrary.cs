using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "RuleTileLibrary", menuName = "Custom/RuleTileLibrary", order = 1)]
public class RuleTileLibrary : ScriptableObject
{
    public List<RuleTileLibraryItem> library;

    public RuleTile GetRuleTile(RuleTileLibraryKey key)
    {
        return library.First(x => x.key == key).tile;
    }
   
}

[Serializable]
public struct RuleTileLibraryItem
{
    public RuleTileLibraryKey key;
    public RuleTile tile;
}

public enum RuleTileLibraryKey
{
    Road,
    CityWall
}