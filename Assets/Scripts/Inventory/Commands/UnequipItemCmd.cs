using System;
using Core;

namespace Ambition
{
	public class UnequipItemCmd : ICommand<ItemVO>
	{
		public void Execute(ItemVO item)
		{
			InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
			ItemVO otherItem;
			if (inventory.Equipped.TryGetValue(item.Slot, out otherItem) && otherItem.ID == item.ID)
			{
				inventory.Equipped.Remove(item.Slot);
				AmbitionApp.SendMessage<ItemVO>(InventoryMessages.UNEQUIPPED, item);
			}
		}
	}

	public class UnequipSlotCmd : ICommand<string>
	{
		public void Execute(string slot)
		{
			InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
			ItemVO item;
			if (inventory.Equipped.TryGetValue(slot, out item))
			{
				inventory.Equipped.Remove(slot);
				AmbitionApp.SendMessage<ItemVO>(InventoryMessages.UNEQUIPPED, item);
			}
		}
	}
}
