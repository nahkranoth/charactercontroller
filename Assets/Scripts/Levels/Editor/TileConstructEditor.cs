using System;
using UnityEditor;
using UnityEngine;

namespace Levels.Editor
{
    [CustomEditor(typeof(TileConstruct))]
    public class TileSetEditor : UnityEditor.Editor
    {
        private TileConstruct construct;
        private void OnEnable()
        {
            construct = target as TileConstruct;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(460f);
            GUILayout.Label(construct.map.Count.ToString());

            GUILayout.BeginArea(new Rect(20,260,200,200));
            Debug.Log(construct.map);
            // for (var y = 0; y < construct.map.y; y++)
            // {
            //     for (int x = 0; x < construct.map.size.x; x++)
            //     {
            //         DrawTexturePreview(new Rect(x*16f, y*16f, 16f, 16f), construct.library[0].sprite);
            //     }
            // }
            GUILayout.EndArea();
        }
        
        private static void DrawTexturePreview(Rect position, Sprite sprite)
        {
            Vector2 fullSize = new Vector2(sprite.texture.width, sprite.texture.height);
            Vector2 size = new Vector2(sprite.textureRect.width, sprite.textureRect.height);

            Rect coords = sprite.textureRect;
            coords.x /= fullSize.x;
            coords.width /= fullSize.x;
            coords.y /= fullSize.y;
            coords.height /= fullSize.y;

            Vector2 ratio;
            ratio.x = position.width/size.x;
            ratio.y = position.height/size.y;

            float minRatio = Mathf.Min(ratio.x, ratio.y);

            Vector2 center = position.center;
            position.width = size.x * minRatio;
            position.height = size.y * minRatio;
            position.center = center;
        
            GUI.DrawTextureWithTexCoords(position, sprite.texture, coords);
        }
    }
}