using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TileSlot))]
public class TileSlotProperty:PropertyDrawer
{
    private TileSlot myTarget;
    private Sprite activeSprite;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        activeSprite = GetValue(property, "sprite") as Sprite;
        SetSprite(property, EditorGUILayout.ObjectField(activeSprite, typeof(Sprite)) as Sprite);
        if(activeSprite == null) return;
        DrawTexturePreview(position, activeSprite);
    }

    private Object GetValue(SerializedProperty property, string name)
    {
        return property.FindPropertyRelative(name).objectReferenceValue;
    }
    
    private void SetSprite(SerializedProperty property, Sprite sprite)
    { 
        property.FindPropertyRelative("sprite").objectReferenceValue = sprite;
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
