using UnityEngine;

public class LevelRepeater : MonoBehaviour
{
    public TilemapBackgroundGenerator bgTmTick;
    public TilemapBackgroundGenerator bgTmTack;

    public void Increase()
    {
        var tg = GetLowest();
        tg.Generate();
        tg.SetPosition(new Vector3(0, tg.tilemap.transform.localPosition.y + tg.tilemap.size.y * 2, 0));
    }
    
    public void Decrease()
    {
        var tg = GetHighest();
        tg.Generate();
        tg.SetPosition(new Vector3(0, tg.tilemap.transform.localPosition.y - tg.tilemap.size.y * 2, 0));
    }

    public TilemapBackgroundGenerator GetLowest()
    {
        if (bgTmTick.GetY() > bgTmTack.GetY()) return bgTmTack;
        return bgTmTick;
    }
    
    public TilemapBackgroundGenerator GetHighest()
    {
        if (bgTmTick.GetY() < bgTmTack.GetY()) return bgTmTack;
        return bgTmTick;
    }
}
