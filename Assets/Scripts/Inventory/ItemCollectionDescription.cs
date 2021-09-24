using System;
using System.Collections.Generic;
using Inventory;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCollectionDescription", menuName = "Custom/ItemCollectionDescription", order = 1)]
public class ItemCollectionDescription : ScriptableObject
{
    public ItemCollection collection;

}
