using System;
using UnityEngine;

[Serializable]
public class Item
{
    public string menuName;
    public Sprite menuSprite;
    public ItemBehaviourStates.Behaviours behaviour;

    public int amount = 1;
}