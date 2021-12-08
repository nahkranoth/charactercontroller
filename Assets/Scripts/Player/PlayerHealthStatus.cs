using System;
using UnityEngine;

public class PlayerHealthStatus:MonoBehaviour
{
    public float thirstTick = 20f;
    private float currentThirstTick = 0f;
    
    public float hungerTick = 40f;
    private float currentHungerTick = 0f;
    
    public float sleepTick = 120f;
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
        itemBehavior.ChangeSleep += AddSleep;
        itemBehavior.ChangeSleep += AddSleep;
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
    }

    public void AddHunger(float amount)
    {
        player.statusController.status.hunger += amount;//normalize
        if (player.statusController.status.hunger > 1f) player.statusController.status.hunger = 1f;
        SetHunger?.Invoke(player.statusController.status.hunger);
    }
    
    public void AddThirst(float amount)
    {
        player.statusController.status.thirst += amount;
        if (player.statusController.status.thirst > 1f) player.statusController.status.thirst = 1f;
        SetThirst?.Invoke(player.statusController.status.thirst);
    }
    
    public void AddSleep(float amount)
    {
        player.statusController.status.sleep += amount;
        if (player.statusController.status.sleep > 1f) player.statusController.status.sleep = 1f;
        SetSleep?.Invoke(player.statusController.status.sleep);
    }

    private void FixedUpdate()
    {
        currentThirstTick += 1f * Time.deltaTime;
        if (currentThirstTick > thirstTick)
        {
            player.statusController.status.thirst -= 0.1f;
            currentThirstTick = 0f;
            SetThirst?.Invoke(player.statusController.status.thirst);
        }
        
        currentHungerTick += 1f * Time.deltaTime;
        if (currentHungerTick > hungerTick)
        {
            player.statusController.status.hunger -= 0.1f;
            currentHungerTick = 0f;
            SetHunger?.Invoke(player.statusController.status.hunger);
        }
        
        currenSleepTick += 1f * Time.deltaTime;
        if (currenSleepTick > sleepTick)
        {
            player.statusController.status.sleep -= 0.1f;
            currenSleepTick = 0f;
            SetSleep?.Invoke(player.statusController.status.sleep);
        }
    }
}
