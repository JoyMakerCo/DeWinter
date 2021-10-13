using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class UnequipItemCmd : ICommand<ItemVO>
	{
        public void Execute(ItemVO item)
        {
            item.Equipped = false;
            AmbitionApp.Inventory.Broadcast();
        }
    }

    public class UnequipItemSlotCmd : ICommand<ItemType>
    {
        public void Execute(ItemType type)
        {
            AmbitionApp.Inventory.Inventory.ForEach(i => { if (i.Type == type) i.Equipped = false; });
            AmbitionApp.Inventory.Broadcast();
        }
    }
}
