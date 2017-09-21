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
				AmbitionApp.GetModel<GameModel>().Livre += item.SellPrice;
				item.SellPrice = 0;
				model.Market.Add(item);
			}
		}
	}
}