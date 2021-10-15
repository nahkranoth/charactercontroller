﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviourController:MonoBehaviour
{
    private Dictionary<ItemBehaviourStates.Behaviours, Action> behaviourToActionMap;
    public Action<ItemBehaviourStates.Behaviours> Equip;
    public Action<int> ChangeHealth;
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
        };
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Chocolate] = () =>
        {
            ChangeHealth?.Invoke(20);
        };
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Honey] = () =>
        {
            ChangeHealth?.Invoke(40);
        };
        
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Axe] = () =>
        {
            Equip?.Invoke(ItemBehaviourStates.Behaviours.Axe);
        };
        behaviourToActionMap[ItemBehaviourStates.Behaviours.Sword] = () =>
        {
            Equip?.Invoke(ItemBehaviourStates.Behaviours.Sword);
        };
    }

    public void Execute(ItemBehaviourStates.Behaviours behaviour)
    {
        behaviourToActionMap[behaviour].Invoke();
    }
}
