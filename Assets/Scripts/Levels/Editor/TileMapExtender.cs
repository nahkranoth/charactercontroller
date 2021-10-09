using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(Tilemap))]
public class TileMapExtender:Editor
{
    private string filename = "Settings/Constructs/Name";
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(30);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Construct Filename");
        filename = GUILayout.TextField(filename);
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Save"))
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
                    nMap.Add(new TileWrapper
                    {
                        position = new Vector3Int(x,y,0), 
                        tile = t
                    });
                }
            }
            tc.map = nMap;
            tc.size = new Vector2Int(tm.size.x, tm.size.y);
            AssetDatabase.CreateAsset(tc, $"Assets/{filename}.asset");
            AssetDatabase.SaveAssets();
        }
    }
}
