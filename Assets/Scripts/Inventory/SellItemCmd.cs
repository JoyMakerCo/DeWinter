using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace DeWinter
{
	public class SellItemCmd : ICommand<ItemVO>
	{
		public void Execute (ItemVO item)
		{
			InventoryModel model = DeWinterApp.GetModel<InventoryModel>();
			List<ItemVO> items;
			if (model.Inventory.TryGetValue(item.Type, out items) && items.Contains(item))
			{
				AdjustValueVO msg = new AdjustValueVO("Livre", item.SellPrice);
				model.Inventory[item.Type].Remove(item);
				DeWinterApp.SendMessage<AdjustValueVO>(msg);
				Debug.Log(item.Name + " Sold for £" + msg.Amount.ToString());
			}
		}
	}
}