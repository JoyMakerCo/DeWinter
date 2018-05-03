using System;
using Core;

namespace Ambition
{
	public class EquipItemCmd : ICommand<ItemVO>
	{
		public void Execute(ItemVO item)
		{
			InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
			AmbitionApp.SendMessage<string>(InventoryMessages.UNEQUIP, item.Slot);
			inventory.Equipped[item.Slot] = item;
			AmbitionApp.SendMessage<ItemVO>(InventoryMessages.EQUIPPED, item);
		}
	}
}
