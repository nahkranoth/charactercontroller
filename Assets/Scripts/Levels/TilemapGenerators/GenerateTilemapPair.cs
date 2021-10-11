using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels.TilemapGenerators
{
    public class GenerateTilemapPair
    {
        public List<Vector3Int> positions = new List<Vector3Int>();
        public List<TileBase> tiles = new List<TileBase>();
    }
}