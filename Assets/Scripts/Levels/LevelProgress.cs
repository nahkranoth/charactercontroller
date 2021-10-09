using System;
using UnityEngine;

public class LevelProgress : MonoBehaviour
{
    public int startSeed;
    public int currentStep;
    public LevelRepeater repeater;

    public int prev_direction = 0;
    private void Start()
    {
        repeater.OnIncrease -= OnIncreaseLevel;
        repeater.OnIncrease += OnIncreaseLevel;
        repeater.OnDecrease -= OnDecreaseLevel;
        repeater.OnDecrease += OnDecreaseLevel;
        
        UpdateSeed();
        repeater.InitTick();
        currentStep++;
        UpdateSeed();
        repeater.InitTack();
        currentStep--;
    }

    private void OnIncreaseLevel()
    {
        currentStep++;
        if (prev_direction == -1) currentStep++;
        UpdateSeed();
        prev_direction = 1;
    }
    
    private void OnDecreaseLevel()
    {
        currentStep--;
        if (prev_direction == 1) currentStep--;
        UpdateSeed();
        prev_direction = -1;
    }
    
    private void UpdateSeed()
    {
        Debug.Log($"Set Seed: {currentStep}");
        UnityEngine.Random.InitState(startSeed+currentStep);
    }
}
