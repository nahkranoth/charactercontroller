using System;
using Inventory;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSettings", menuName = "Custom/ItemSettings", order = 1)]
public class ItemDescription : ScriptableObject
{
    public Item item;
}

