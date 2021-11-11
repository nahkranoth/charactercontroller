using System;
using UnityEngine;

public class PlayerHealthStatus:MonoBehaviour
{
    public float thirstTick = 4f;
    private float currentThirstTick = 0f;
    
    public float hungerTick = 10f;
    private float currentHungerTick = 0f;
    
    public float sleepTick = 30f;
    private float currenSleepTick = 0f;

    private ItemBehaviourController itemBehavior;
    private PlayerController player;
    
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
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
    }

    public void AddHunger(float amount)
    {
        player.status.hunger += amount;//normalize
        if (player.status.hunger > 1f) player.status.hunger = 1f;
        SetHunger?.Invoke(player.status.hunger);
    }
    
    public void AddThirst(float amount)
    {
        player.status.thirst += amount;
        if (player.status.thirst > 1f) player.status.thirst = 1f;
        SetThirst?.Invoke(player.status.thirst);
    }
    
    public void AddSleep(float amount)
    {
        player.status.sleep += amount;
        if (player.status.sleep > 1f) player.status.sleep = 1f;
        SetSleep?.Invoke(player.status.sleep);
    }

    private void FixedUpdate()
    {
        currentThirstTick += 1f * Time.deltaTime;
        if (currentThirstTick > thirstTick)
        {
            player.status.thirst -= 0.1f;
            currentThirstTick = 0f;
            SetThirst?.Invoke(player.status.thirst);
        }
        
        currentHungerTick += 1f * Time.deltaTime;
        if (currentHungerTick > hungerTick)
        {
            player.status.hunger -= 0.1f;
            currentHungerTick = 0f;
            SetHunger?.Invoke(player.status.hunger);
        }
        
        currenSleepTick += 1f * Time.deltaTime;
        if (currenSleepTick > sleepTick)
        {
            player.status.sleep -= 0.1f;
            currenSleepTick = 0f;
            SetSleep?.Invoke(player.status.sleep);
        }
    }
}
