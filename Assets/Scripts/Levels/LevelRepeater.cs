using UnityEngine;

public class LevelRepeater : MonoBehaviour
{
    public TilemapGenerator tilemapGenOne;
    public TilemapGenerator tilemapGenTwo;

    public bool increase;

    private void OnValidate()
    {
        Increase();
    }

    public void Increase()
    {
        var tg = GetLowest();
        tg.Generate();
        tg.tilemap.transform.localPosition = new Vector3(0, tg.tilemap.transform.localPosition.y + tg.tilemap.size.y, 0);
    }
    
    public void Decrease()
    {
        var tg = GetHighest();
        tg.Generate();
        tg.tilemap.transform.localPosition = new Vector3(0, tg.tilemap.transform.localPosition.y - tg.tilemap.size.y, 0);
    }

    public TilemapGenerator GetLowest()
    {
        if (tilemapGenOne.tilemap.transform.position.y > tilemapGenTwo.tilemap.transform.position.y) return tilemapGenTwo;
        return tilemapGenOne;
    }
    
    public TilemapGenerator GetHighest()
    {
        if (tilemapGenOne.tilemap.transform.position.y < tilemapGenTwo.tilemap.transform.position.y) return tilemapGenTwo;
        return tilemapGenOne;
    }
}
