using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels
{
    public abstract class TilemapGenerator : MonoBehaviour
    {
        public Vector2Int size;
        public Tilemap tilemap;
        internal Dictionary<int, TileBase> tileBaseMap = new Dictionary<int, TileBase>();
        
        public Action<Tilemap> OnDoneInit;
        
        internal void InitBlueprint(ref int[,] map)
        {
            for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
                map[x, y] = 1;
        }
        
        internal void BuildMap(int[,] map)
        {
            tilemap.ClearAllTiles();
            TileBase tb;
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {
                    tileBaseMap.TryGetValue(map[x, y], out tb);
                    tilemap.SetTile(new Vector3Int(x - size.x/2, y - size.y/2, 0), tb);
                }
            }
            OnDoneInit?.Invoke(tilemap);
        }
    }
}