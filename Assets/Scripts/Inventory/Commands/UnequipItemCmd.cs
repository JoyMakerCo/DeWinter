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
            if (inventory.Equipped.TryGetValue(item.Type, out otherItem) && otherItem.ID == item.ID)
			{
                inventory.Equipped.Remove(item.Type);
				AmbitionApp.SendMessage(InventoryMessages.UNEQUIPPED, item);
			}
		}
	}

	public class UnequipSlotCmd : ICommand<string>
	{
        public void Execute(string type)
		{
			InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
			ItemVO item;
			if (inventory.Equipped.TryGetValue(type, out item) && item != null)
			{
				inventory.Equipped.Remove(type);
				AmbitionApp.SendMessage(InventoryMessages.UNEQUIPPED, item);
			}
		}
	}
}
