using UnityEngine;

public class LevelRepeatDetector : MonoBehaviour
{
    public CameraFollow camera;
    public LevelRepeater repeater;

    public int incOffset;
    public int decOffset;
    
    private void Update()
    {
        var highest = repeater.GetHighest();
        var lowest = repeater.GetLowest();
        if (camera.transform.position.y > highest.GetY() + incOffset)
        {
            repeater.Increase();
            return;
        }

        if (camera.transform.position.y < lowest.GetY() - decOffset)
        {
            repeater.Decrease();
        }
    }
}
