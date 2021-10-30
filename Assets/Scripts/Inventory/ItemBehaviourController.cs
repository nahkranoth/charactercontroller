using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviourController:MonoBehaviour
{
    private Dictionary<ItemBehaviourStates.Behaviours, Action> behaviourToActionMap;
    public Action<ItemBehaviourStates.Behaviours> Equip;
    public Action<int> ChangeHealth;
    public Action<float> ChangeHunger;
    public Action<float> ChangeThirst;
    private void Awake()
    {
        behaviourToActionMap = new Dictionary<ItemBehaviourStates.Behaviours, Action>();
        WorldGraph.Subscribe(this, typeof(ItemBehaviourController));
    }

    private void Start()
    {
        behaviourToActionMap[ItemBehaviourStates.Behaviours.None] = () => { Debug.Log("Do nothing");};
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Candy] = () =>
        {
            ChangeHealth?.Invoke(10);
            ChangeHunger?.Invoke(30);
        };
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Chocolate] = () =>
        {
            ChangeHealth?.Invoke(20);
            ChangeHunger?.Invoke(40);

        };
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Honey] = () =>
        {
            ChangeHealth?.Invoke(40);
            ChangeHunger?.Invoke(60);
            ChangeThirst?.Invoke(10);
        };
        
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Axe] = () =>
        {
            Equip?.Invoke(ItemBehaviourStates.Behaviours.Axe);
        };
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Sword] = () =>
        {
            Equip?.Invoke(ItemBehaviourStates.Behaviours.Sword);
        };
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Torch] = () =>
        {
            Equip?.Invoke(ItemBehaviourStates.Behaviours.Torch);
        };
        behaviourToActionMap[ItemBehaviourStates.Behaviours.WaterBottle] = () =>
        {
            ChangeThirst?.Invoke(80);
        };
    }

    public void Execute(ItemBehaviourStates.Behaviours behaviour)
    {
        Action action;
        behaviourToActionMap.TryGetValue(behaviour, out action); 
        if (action != null) action.Invoke();
    }
}
