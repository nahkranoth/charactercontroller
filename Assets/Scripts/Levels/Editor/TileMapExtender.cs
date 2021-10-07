using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(Tilemap))]
public class TileMapExtender:Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Save as template"))
        {
            var tm = target as Tilemap;
            TileConstruct tc = CreateInstance<TileConstruct>();
            var nMap = new List<TileWrapper>();
            for (int x = 0; x < tm.size.x; x++)
            {
                for (int y = 0; y < tm.size.y; y++)
                {
                    var t = tm.GetTile(new Vector3Int(x, y, 0)) as Tile;
                    if(t == null) continue;
                    nMap.Add(new TileWrapper(){position = new Vector3Int(x,y,0), tile = t});
                }
            }
            tc.map = nMap;
            AssetDatabase.CreateAsset(tc, "Assets/New2.asset");
            AssetDatabase.SaveAssets();
        }
    }
}
