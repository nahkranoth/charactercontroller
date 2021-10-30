using System;
using UnityEngine;

[Serializable]
public class Item
{
    public string menuName;
    public Sprite menuSprite;
    public Sprite equipedSprite;
    public ItemBehaviourStates.Behaviours behaviour;
    
    public bool consumable;
    public int amount = 1;

    public int price;
    
    public bool canChopWood;

    public Item DeepCopy()
    {
        return MemberwiseClone() as Item;
    }

}