using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviourController:MonoBehaviour
{
    private Dictionary<ItemBehaviourStates.Behaviours, Action> behaviourToActionMap;
    public Action<ItemBehaviourStates.Behaviours> Equip;
    public Action<ItemBehaviourStates.Behaviours> Consume;
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
            Debug.Log("Candy");
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
