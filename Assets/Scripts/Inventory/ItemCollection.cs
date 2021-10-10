using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory
{
    [Serializable]
    public class ItemCollection
    {
        public List<ItemDescription> descriptions;

        public ItemDescription FindByBehaviours(ItemBehaviourStates.Behaviours search)
        {
            return descriptions.First(x => x.item.behaviour == search);
        }
        
    }
}