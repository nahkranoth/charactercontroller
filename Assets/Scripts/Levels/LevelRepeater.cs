using UnityEngine;

public class LevelRepeater : MonoBehaviour
{
    public TilemapBackgroundGenerator tilemapBackgroundGenOne;
    public TilemapBackgroundGenerator tilemapBackgroundGenTwo;

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
        if (tilemapBackgroundGenOne.GetY() > tilemapBackgroundGenTwo.GetY()) return tilemapBackgroundGenTwo;
        return tilemapBackgroundGenOne;
    }
    
    public TilemapBackgroundGenerator GetHighest()
    {
        if (tilemapBackgroundGenOne.GetY() < tilemapBackgroundGenTwo.GetY()) return tilemapBackgroundGenTwo;
        return tilemapBackgroundGenOne;
    }
}
