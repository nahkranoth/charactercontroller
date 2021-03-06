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
    public bool equipable;
    public bool wearable;
    public float armor;
    public SentientBodyState bodySettings;

    public int price;
    
    public bool canChopWood;
    public int damage;

    public int nourishment;
    public int healing;
    public int thirst;

    public float weight;

    public GameObject spawnable;

    
    [HideInInspector] public int amount = 1;

    public Item DeepCopy()
    {
        return MemberwiseClone() as Item;
    }

}