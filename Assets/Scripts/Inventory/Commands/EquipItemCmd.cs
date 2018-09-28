using System;
using Core;

namespace Ambition
{
	public class EquipItemCmd : ICommand<ItemVO>
	{
		public void Execute(ItemVO item)
		{
			InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            AmbitionApp.SendMessage(InventoryMessages.UNEQUIP, item.Type);
            inventory.Equipped[item.Type] = item;
			AmbitionApp.SendMessage(InventoryMessages.EQUIPPED, item);
		}
	}
}
