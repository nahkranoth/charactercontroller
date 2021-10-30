using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityInventory
{
       public List<Item> storage = new List<Item>();
       [SerializeField] private int money;

       public Action<int> OnMoneyChange;
       
       public int Money
       {
           get { return money; }
       }
       
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
       
       
       public void ChangeMoney(int amount)
       {
           money += amount;
           OnMoneyChange?.Invoke(money);
       }
}
