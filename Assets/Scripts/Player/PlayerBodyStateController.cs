using System;
using UnityEngine;

public class PlayerBodyStateController:MonoBehaviour
{
    private ItemBehaviourController itemBehavior;
    private PlayerController player;
    
    public Action<float> SetThirst;
    public Action<float> SetHunger;
    public Action<float> SetSleep;

    public SentientBodyStateModifier bodyStateModifier;

    private void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        itemBehavior = WorldGraph.Retrieve(typeof(ItemBehaviourController)) as ItemBehaviourController;
        itemBehavior.ChangeHunger -= AddHunger;
        itemBehavior.ChangeHunger += AddHunger;
        itemBehavior.ChangeThirst -= AddThirst;
        itemBehavior.ChangeThirst += AddThirst;
        itemBehavior.ChangeSleep -= AddSleep;
        itemBehavior.ChangeSleep += AddSleep;
        
        bodyStateModifier.HungerTick += () => { AddHunger(-0.1f); };
        bodyStateModifier.ThirstTick += () => { AddThirst(-0.1f); };
        bodyStateModifier.SleepTick += () => { AddSleep(-0.1f); };
    }

    public void AddHunger(float amount)
    {
        player.stateController.Hunger += amount;//normalized
        player.stateController.Hunger = Mathf.Min(1f, player.stateController.Hunger);//Cap to 1
        SetHunger?.Invoke(player.stateController.Hunger);
    }
    
    public void AddThirst(float amount)
    {
        player.stateController.Thirst += amount;
        player.stateController.Thirst = Mathf.Min(1f, player.stateController.Thirst);
        SetThirst?.Invoke(player.stateController.Thirst);
    }
    
    public void AddSleep(float amount)
    {
        player.stateController.Sleep += amount;
        player.stateController.Sleep = Mathf.Min(1f, player.stateController.Sleep);
        SetSleep?.Invoke(player.stateController.Sleep);
    }
}
