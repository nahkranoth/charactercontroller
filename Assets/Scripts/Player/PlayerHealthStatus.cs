using System;
using UnityEngine;

public class PlayerHealthStatus:MonoBehaviour
{
    public Health myHealth;

    public float hunger = 1f;
    public float thirst = 1f;
    public float sleep = 1f;

    public float thirstTick = 4f;
    private float currentThirstTick = 0f;
    
    public float hungerTick = 10f;
    private float currentHungerTick = 0f;
    
    public float sleepTick = 30f;
    private float currenSleepTick = 0f;

    public Action<float> DecreaseThirst;
    public Action<float> DecreaseHunger;
    public Action<float> DecreaseSleep;
    private void Awake()
    {
        WorldGraph.Subscribe(this,typeof(PlayerHealthStatus));
    }

    private void FixedUpdate()
    {
        currentThirstTick += 1f * Time.deltaTime;
        if (currentThirstTick > thirstTick)
        {
            thirst -= 0.1f;
            currentThirstTick = 0f;
            DecreaseThirst?.Invoke(thirst);
        }
        
        currentHungerTick += 1f * Time.deltaTime;
        if (currentHungerTick > hungerTick)
        {
            hunger -= 0.1f;
            currentHungerTick = 0f;
            DecreaseHunger?.Invoke(hunger);
        }
        
        currenSleepTick += 1f * Time.deltaTime;
        if (currenSleepTick > sleepTick)
        {
            sleep -= 0.1f;
            currenSleepTick = 0f;
            DecreaseSleep?.Invoke(sleep);
        }
    }
}
