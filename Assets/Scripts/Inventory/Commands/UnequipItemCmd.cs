using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class UnequipItemCmd : ICommand<ItemVO>
	{
        public void Execute(ItemVO item)
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            item.Equipped = false;
            if (inventory.Equipped.TryGetValue(item.Type, out ItemVO equipped) && equipped == item)
            {
                inventory.Equipped.Remove(item.Type);
            }
            inventory.Inventory.Add(item);
            inventory.Broadcast();
        }
    }

    public class UnequipItemSlotCmd : ICommand<ItemType>
    {
        public void Execute(ItemType type)
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            if (inventory.Equipped.TryGetValue(type, out ItemVO equipped))
            {
                equipped.Equipped = false;
                inventory.Equipped.Remove(type);
                inventory.Inventory.Add(equipped);
                inventory.Broadcast();
            }
        }
    }
}
