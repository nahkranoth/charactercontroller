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
    public Action<GameObject> SpawnEntity;
    private void Awake()
    {
        behaviourToActionMap = new Dictionary<ItemBehaviourStates.Behaviours, Action<Item>>();
        WorldGraph.Subscribe(this, typeof(ItemBehaviourController));
    }

    private void Start()
    {
        behaviourToActionMap[ItemBehaviourStates.Behaviours.None] = (item) => { Debug.Log("Do nothing");};
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Candy] = (item) =>
        {
            ChangeHealth?.Invoke(10);
            ChangeHunger?.Invoke(30);
        };
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Chocolate] = (item) =>
        {
            ChangeHealth?.Invoke(20);
            ChangeHunger?.Invoke(40);

        };
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Honey] = (item) =>
        {
            ChangeHealth?.Invoke(40);
            ChangeHunger?.Invoke(60);
            ChangeThirst?.Invoke(10);
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
        behaviourToActionMap[ItemBehaviourStates.Behaviours.WaterBottle] = (item) =>
        {
            ChangeThirst?.Invoke(80);
        };
        behaviourToActionMap[ItemBehaviourStates.Behaviours.DonkeySpawner] = (item) =>
        {
            SpawnEntity?.Invoke(item.spawnable);
        };
    }

    public void Execute(Item item)
    {
        Action<Item> action;
        behaviourToActionMap.TryGetValue(item.behaviour, out action); 
        if (action != null) action.Invoke(item);
    }
}
