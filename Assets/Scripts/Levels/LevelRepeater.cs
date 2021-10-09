using System;
using UnityEngine;

public class LevelRepeater : MonoBehaviour
{
    public MetaTilemapGenerator metaTilemapTick;
    public MetaTilemapGenerator metaTilemapTack;

    public Action OnIncrease;
    public Action OnDecrease;

    public void InitTick()
    {
        metaTilemapTick.Generate();
    }
    
    public void InitTack()
    {
        metaTilemapTack.Generate();
    }
    
    public void Increase()
    {
        var tg = GetLowest();
        OnIncrease?.Invoke();
        tg.Generate();
        var yPos = tg.background.tilemap.transform.localPosition.y + (tg.tilemapSize.y - 1) * 2;
        tg.SetPosition(new Vector3(0, yPos, 0));
    }
    
    public void Decrease()
    {
        var tg = GetHighest();
        OnDecrease?.Invoke();
        tg.Generate();
        var yPos = tg.background.tilemap.transform.localPosition.y - tg.background.tilemap.size.y * 2;
        tg.SetPosition(new Vector3(0, yPos, 0));
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
