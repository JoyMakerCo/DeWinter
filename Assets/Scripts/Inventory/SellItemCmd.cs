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
                int price = item.Price;
                price = (int)((float)item.Price * model.SellbackMultiplier);
                AmbitionApp.GetModel<GameModel>().Livre += price;
				model.Market.Add(item);
			}
		}
	}
}
