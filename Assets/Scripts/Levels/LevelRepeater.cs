using UnityEngine;

public class LevelRepeater : MonoBehaviour
{
    public MetaTilemapGenerator metaTilemapTick;
    public MetaTilemapGenerator metaTilemapTack;

    public void Increase()
    {
        var tg = GetLowest();
        tg.Generate();
        tg.SetPosition(new Vector3(0, tg.background.tilemap.transform.localPosition.y + (tg.tilemapSize.y-1) * 2, 0));
    }
    
    public void Decrease()
    {
        var tg = GetHighest();
        tg.Generate();
        tg.SetPosition(new Vector3(0, tg.background.tilemap.transform.localPosition.y - tg.background.tilemap.size.y * 2, 0));
    }

    public MetaTilemapGenerator GetLowest()
    {
        if (metaTilemapTick.GetY() > metaTilemapTack.GetY()) return metaTilemapTack;
        return metaTilemapTick;
    }
    
    public MetaTilemapGenerator GetHighest()
    {
        if (metaTilemapTick.GetY() < metaTilemapTack.GetY()) return metaTilemapTack;
        return metaTilemapTick;
    }
}
