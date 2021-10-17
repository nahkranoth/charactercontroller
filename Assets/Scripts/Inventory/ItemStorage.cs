using System.Collections.Generic;

public class ItemStorage
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
           var possibleItem = new Item
           {
               behaviour = description.item.behaviour,
               menuName = description.item.menuName,
               menuSprite = description.item.menuSprite,
               equipedSprite = description.item.equipedSprite,
               consumable = description.item.consumable,
               destroysBlocks = description.item.destroysBlocks
           };

           var storageLocation = storage.Find(x => x.menuName == possibleItem.menuName);

           if (storageLocation != null)
           {
               storageLocation.amount += 1;
               return;
           }
           
           storage.Add(new Item
           {
              behaviour = description.item.behaviour,
              menuName = description.item.menuName,
              menuSprite = description.item.menuSprite,
              equipedSprite = description.item.equipedSprite,
              consumable = description.item.consumable,
              destroysBlocks = description.item.destroysBlocks
           });
       }

       public void AddMultipleByDescription(List<ItemDescription> descriptions)
       {
           foreach (var description in descriptions)
           {
               AddByDescription(description);
           }
       }
}
