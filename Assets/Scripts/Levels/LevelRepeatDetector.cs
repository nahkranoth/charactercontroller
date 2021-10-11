using UnityEngine;

public class LevelRepeatDetector : MonoBehaviour
{
    public PlayerController player;
    public LevelRepeater repeater;

    public int incOffset;
    public int decOffset;
    
    private void Update()
    {
        var highest = repeater.GetHighestGeneratePoint();
        var lowest = repeater.GetLowestGeneratePoint();
        if (player.transform.localPosition.y > highest - incOffset)
        {
            repeater.Increase();
            return;
        }
        
        if (player.transform.localPosition.y < lowest + decOffset)
        {
            repeater.Decrease();
        }
    }
}
