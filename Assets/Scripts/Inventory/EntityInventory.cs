using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityInventory
{
       public List<Item> storage = new List<Item>();

       public bool TakeItem(Item item)
       {
           var itm = storage.Find(x => x == item);
           if (itm == null) return false;
           itm.amount--;
           if (itm.amount == 0)
           {
               storage.Remove(itm);
               return true;//IS EMPTY
           }
           return false;
       }

       public Item FindByBehaviour(ItemBehaviourStates.Behaviours behave)
       {
           return storage.Find(x => x.behaviour == behave);
       }
       
       public void AddByDescription(ItemDescription description)
       {
           var possibleItem = description.item.DeepCopy();

           var storageLocation = storage.Find(x => x.menuName == possibleItem.menuName);

           if (storageLocation != null)
           {
               storageLocation.amount += 1;
               return;
           }
           
           storage.Add(possibleItem);
       }

       public void RemoveByItem(Item _item)
       {
           var storageLocation = storage.Find(x => x.menuName == _item.menuName);
           
           if (storageLocation != null && storageLocation.amount > 1)
           {
               storageLocation.amount -= 1;
               return;
           }
           
           storage.Remove(_item);
       }

       public bool Exists(Item _item)
       {
           var storageLocation = storage.Find(x => x.menuName == _item.menuName);
           return storageLocation != null;
       }
       
       public void AddByItem(Item _item)
       {

           var storageLocation = storage.Find(x => x.menuName == _item.menuName);

           if (storageLocation != null)
           {
               storageLocation.amount += 1;
               return;
           }
           
           storage.Add(_item);
       }

       public void AddMultipleByDescription(List<ItemDescription> descriptions)
       {
           foreach (var description in descriptions)
           {
               AddByDescription(description);
           }
       }

       public float TotalItemWeight()
       {
           float total = 0f;
           foreach (var item in storage)
           {
               total += item.weight;
           }

           return total;
       }
    
}
