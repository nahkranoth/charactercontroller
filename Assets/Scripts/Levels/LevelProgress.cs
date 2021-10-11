using UnityEngine;

public class LevelProgress : MonoBehaviour
{
    public int startSeed;
    public int currentStep;
    public LevelRepeater repeater;
    private void Start()
    {
        UpdateSeed();
        repeater.OnInit();
    }
    
    private void UpdateSeed()
    {
        Debug.Log($"Set Seed: {currentStep}");
        Random.InitState(startSeed+currentStep);
    }
}
