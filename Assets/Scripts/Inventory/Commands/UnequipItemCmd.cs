using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class UnequipItemCmd : ICommand<ItemVO>
	{
        public void Execute(ItemVO item) => AmbitionApp.GetModel<InventoryModel>().Unequip(item);
    }

	public class UnequipSlotCmd : ICommand<ItemType>
	{
        public void Execute(ItemType type) => AmbitionApp.GetModel<InventoryModel>().Unequip(type);
    }
}
