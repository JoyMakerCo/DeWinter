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
			if (model.Inventory.Remove(item))
			{
				AdjustValueVO msg = new AdjustValueVO("Livre", item.SellPrice);
				DeWinterApp.AdjustValue<>(msg);
				Debug.Log(item.Name + " Sold for £" + msg.Amount.ToString());
			}
		}
	}
}