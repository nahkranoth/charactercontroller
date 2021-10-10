using System;
using UnityEngine;

public class LevelRepeater : MonoBehaviour
{
    public MetaTilemapGenerator metaTilemapTick;
    public MetaTilemapGenerator metaTilemapTack;
    public MetaLevelEntityPlacer metaEntityPlacer;
    
    public Action OnIncrease;
    public Action OnDecrease;

    public void InitTick()
    {
        metaTilemapTick.Generate();
        metaEntityPlacer.GenerateTick(metaTilemapTick);
    }
    
    public void InitTack()
    {
        metaTilemapTack.Generate();
        metaEntityPlacer.GenerateTack(metaTilemapTack);
    }
    
    public void Increase()
    {
        var lowestTilemapGenerator = GetLowestTilemap();
        var lowestEntitySpawner = GetLowestEntitySpawner();

        OnIncrease?.Invoke();
        lowestTilemapGenerator.Generate();
        var yPos = lowestTilemapGenerator.background.Position.y + (lowestTilemapGenerator.tilemapSize.y - 1) * 2;
        lowestTilemapGenerator.SetPosition(new Vector3(0, yPos, 0));
        
        metaEntityPlacer.Generate(lowestEntitySpawner, lowestTilemapGenerator);
    }
    
    public void Decrease()
    {
        var highestTilemapGenerator = GetHighestTilemap();
        var highestEntitySpawner = GetHighestEntitySpawner();

        OnDecrease?.Invoke();
        highestTilemapGenerator.Generate();
        var yPos = highestTilemapGenerator.background.tilemap.transform.localPosition.y - highestTilemapGenerator.background.tilemap.size.y * 2;
        highestTilemapGenerator.SetPosition(new Vector3(0, yPos, 0));

        metaEntityPlacer.Generate(highestEntitySpawner, highestTilemapGenerator);
    }

    public MetaTilemapGenerator GetLowestTilemap()
    {
        if (metaTilemapTick.GetY() > metaTilemapTack.GetY()) return metaTilemapTack;
        return metaTilemapTick;
    }
    
    public MetaTilemapGenerator GetHighestTilemap()
    {
        if (metaTilemapTick.GetY() < metaTilemapTack.GetY()) return metaTilemapTack;
        return metaTilemapTick;
    }
    
    public LevelEntityPlacer GetLowestEntitySpawner()
    {
        if (metaTilemapTick.GetY() > metaTilemapTack.GetY()) return metaEntityPlacer.entityPlacerTack;
        return metaEntityPlacer.entityPlacerTick;
    }
    
    public LevelEntityPlacer GetHighestEntitySpawner()
    {
        if (metaTilemapTick.GetY() < metaTilemapTack.GetY()) return metaEntityPlacer.entityPlacerTack;
        return metaEntityPlacer.entityPlacerTick;
    }

}
