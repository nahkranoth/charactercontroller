using System;
using UnityEngine;

public class PlayerHealthStatus:MonoBehaviour
{
    public Health myHealth;

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
        myHealth.hunger += amount;//normalize
        if (myHealth.hunger > 1f) myHealth.hunger = 1f;
        SetHunger?.Invoke(myHealth.hunger);
    }
    
    public void AddThirst(float amount)
    {
        myHealth.thirst += amount;
        if (myHealth.thirst > 1f) myHealth.thirst = 1f;
        SetThirst?.Invoke(myHealth.thirst);
    }
    
    public void AddSleep(float amount)
    {
        myHealth.sleep += amount;
        if (myHealth.sleep > 1f) myHealth.sleep = 1f;
        SetSleep?.Invoke(myHealth.sleep);
    }

    private void FixedUpdate()
    {
        currentThirstTick += 1f * Time.deltaTime;
        if (currentThirstTick > thirstTick)
        {
            myHealth.thirst -= 0.1f;
            currentThirstTick = 0f;
            SetThirst?.Invoke(myHealth.thirst);
        }
        
        currentHungerTick += 1f * Time.deltaTime;
        if (currentHungerTick > hungerTick)
        {
            myHealth.hunger -= 0.1f;
            currentHungerTick = 0f;
            SetHunger?.Invoke(myHealth.hunger);
        }
        
        currenSleepTick += 1f * Time.deltaTime;
        if (currenSleepTick > sleepTick)
        {
            myHealth.sleep -= 0.1f;
            currenSleepTick = 0f;
            SetSleep?.Invoke(myHealth.sleep);
        }
    }
}
