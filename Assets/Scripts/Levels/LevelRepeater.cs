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
        var lowestTilemapGenerator = GetLowest();
        OnIncrease?.Invoke();
        lowestTilemapGenerator.Generate();
        var yPos = lowestTilemapGenerator.background.Position.y + (lowestTilemapGenerator.tilemapSize.y - 1) * 2;
        lowestTilemapGenerator.SetPosition(new Vector3(0, yPos, 0));
    }
    
    public void Decrease()
    {
        var highestTilemapGenerator = GetHighest();
        OnDecrease?.Invoke();
        highestTilemapGenerator.Generate();
        var yPos = highestTilemapGenerator.background.tilemap.transform.localPosition.y - highestTilemapGenerator.background.tilemap.size.y * 2;
        highestTilemapGenerator.SetPosition(new Vector3(0, yPos, 0));
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
