using System;
using UnityEngine;

public class LevelProgress : MonoBehaviour
{
    public int startSeed;
    public int currentStep;
    public LevelRepeater repeater;
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
    }
    
    //TODO There is an error in here with the start state actually loading in two states putting the currentstep on 1
    
    private void OnIncreaseLevel()
    {
        currentStep++;
        UpdateSeed();
    }
    
    private void OnDecreaseLevel()
    {
        currentStep--;
        UpdateSeed();
    }
    
    private void UpdateSeed()
    {
        Debug.Log($"Set Seed: {currentStep}");
        UnityEngine.Random.InitState(startSeed+currentStep);
    }
}
