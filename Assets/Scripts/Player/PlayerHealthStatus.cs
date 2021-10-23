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

    private ItemBehaviourController itemBehavior;
    
    public Action<float> SetThirst;
    public Action<float> SetHunger;
    public Action<float> SetSleep;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this,typeof(PlayerHealthStatus));
    }

    private void Start()
    {
        itemBehavior = WorldGraph.Retrieve(typeof(ItemBehaviourController)) as ItemBehaviourController;
        itemBehavior.ChangeHunger -= AddHunger;
        itemBehavior.ChangeHunger += AddHunger;
        itemBehavior.ChangeThirst -= AddThirst;
        itemBehavior.ChangeThirst += AddThirst;
    }

    public void AddHunger(float amount)
    {
        hunger += amount;//normalize
        if (hunger > 1f) hunger = 1f;
        SetHunger?.Invoke(hunger);
    }
    
    public void AddThirst(float amount)
    {
        thirst += amount;
        if (thirst > 1f) thirst = 1f;
        SetThirst?.Invoke(thirst);
    }
    
    public void AddSleep(float amount)
    {
        sleep += amount;
        if (sleep > 1f) sleep = 1f;
        SetSleep?.Invoke(sleep);
    }

    private void FixedUpdate()
    {
        currentThirstTick += 1f * Time.deltaTime;
        if (currentThirstTick > thirstTick)
        {
            thirst -= 0.1f;
            currentThirstTick = 0f;
            SetThirst?.Invoke(thirst);
        }
        
        currentHungerTick += 1f * Time.deltaTime;
        if (currentHungerTick > hungerTick)
        {
            hunger -= 0.1f;
            currentHungerTick = 0f;
            SetHunger?.Invoke(hunger);
        }
        
        currenSleepTick += 1f * Time.deltaTime;
        if (currenSleepTick > sleepTick)
        {
            sleep -= 0.1f;
            currenSleepTick = 0f;
            SetSleep?.Invoke(sleep);
        }
    }
}
