using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Ambition
{
	public class SellItemCmd : ICommand<ItemVO>
	{
		public void Execute (ItemVO item)
		{
			InventoryModel model = AmbitionApp.GetModel<InventoryModel>();
			if (model.Inventory.Remove(item))
			{
				AdjustValueVO msg = new AdjustValueVO("Livre", item.SellPrice);
				AmbitionApp.SendMessage<AdjustValueVO>(msg);
				Debug.Log(item.Name + " Sold for £" + msg.Amount.ToString());
			}
		}
	}
}