using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviourController:MonoBehaviour
{
    private Dictionary<ItemBehaviourStates.Behaviours, Action<Item>> behaviourToActionMap;
    public Action<ItemBehaviourStates.Behaviours> Equip;
    public Action<int> ChangeHealth;
    public Action<float> ChangeHunger;
    public Action<float> ChangeThirst;
    public Action<float> ChangeSleep;
    public Action<GameObject> SpawnEntity;
    public Action<GameObject> SpawnWorldItem;
    private void Awake()
    {
        behaviourToActionMap = new Dictionary<ItemBehaviourStates.Behaviours, Action<Item>>();
        WorldGraph.Subscribe(this, typeof(ItemBehaviourController));
    }

    private void Start()
    {
        behaviourToActionMap[ItemBehaviourStates.Behaviours.None] = (item) => { Debug.Log("Do nothing");};
        behaviourToActionMap[ItemBehaviourStates.Behaviours.SimpleFood] = (item) =>
        {
            ChangeHealth?.Invoke(item.healing);
            ChangeHunger?.Invoke(item.nourishment);
            ChangeThirst?.Invoke(item.thirst);
        };
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Axe] = (item) =>
        {
            Equip?.Invoke(ItemBehaviourStates.Behaviours.Axe);
        };
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Sword] = (item) =>
        {
            Equip?.Invoke(ItemBehaviourStates.Behaviours.Sword);
        };
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Torch] = (item) =>
        {
            Equip?.Invoke(ItemBehaviourStates.Behaviours.Torch);
        };
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Spawnable] = (item) =>
        {
            SpawnEntity?.Invoke(item.spawnable);
        };
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Tent] = (item) =>
        {
            SpawnWorldItem?.Invoke(item.spawnable);
            ChangeSleep?.Invoke(1f);
            var worldTime = WorldGraph.Retrieve(typeof(WorldTimeController)) as WorldTimeController;
            worldTime.SpeedToMorning();
        };
        
    }

    public void Execute(Item item)
    {
        Action<Item> action;
        behaviourToActionMap.TryGetValue(item.behaviour, out action); 
        if (action != null) action.Invoke(item);
    }
}
