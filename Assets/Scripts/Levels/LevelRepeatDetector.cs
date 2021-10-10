using UnityEngine;

public class LevelRepeatDetector : MonoBehaviour
{
    public PlayerController player;
    public LevelRepeater repeater;

    public int incOffset;
    public int decOffset;
    
    private void Update()
    {
        var highest = repeater.GetHighestTilemap();
        var lowest = repeater.GetLowestTilemap();
        if (player.transform.position.y > highest.GetY() + incOffset)
        {
            repeater.Increase();
            return;
        }

        if (player.transform.position.y < lowest.GetY() - decOffset)
        {
            repeater.Decrease();
        }
    }
}
