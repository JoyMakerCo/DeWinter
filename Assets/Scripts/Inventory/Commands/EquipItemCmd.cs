using System;
using Core;

namespace Ambition
{
	public class EquipItemCmd : ICommand<ItemVO>
	{
		public void Execute(ItemVO item)
		{
			InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
			ItemVO otherItem;
			if (inventory.Equipped.TryGetValue(item.Slot, out otherItem))
			{
				AmbitionApp.SendMessage<ItemVO>(InventoryMessages.ITEM_UNEQUIPPED, otherItem);
			}
			inventory.Equipped[item.Slot] = item;
			AmbitionApp.SendMessage<ItemVO>(InventoryMessages.ITEM_EQUIPPED, otherItem);
		}
	}
}
