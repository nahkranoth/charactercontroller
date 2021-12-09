using System;
using UnityEngine;

public class SentientBodyStateModifier:MonoBehaviour
{
    [SerializeField] private float thirstTick = 20f;
    private float currentThirstTick = 0f;
    
    [SerializeField] private float hungerTick = 40f;
    private float currentHungerTick = 0f;
    
    [SerializeField] private float sleepTick = 120f;
    private float currenSleepTick = 0f;
    
    public Action ThirstTick;
    public Action HungerTick;
    public Action SleepTick;

    private void FixedUpdate()
    {
        currentThirstTick += Time.deltaTime;
        if (currentThirstTick > thirstTick)
        {
            currentThirstTick = 0f;
            ThirstTick.Invoke();
        }
    
        currentHungerTick += Time.deltaTime;
        if (currentHungerTick > hungerTick)
        {
            currentHungerTick = 0f;
            HungerTick.Invoke();
        }
    
        currenSleepTick += Time.deltaTime;
        if (currenSleepTick > sleepTick)
        {
            currenSleepTick = 0f;
            SleepTick.Invoke();
        }
    }
}
