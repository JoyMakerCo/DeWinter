using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Ambition
{
	public class BuyItemCmd : ICommand<ItemVO>
	{
		public void Execute (ItemVO item)
		{
			InventoryModel model = AmbitionApp.GetModel<InventoryModel>();
			if (model.Inventory.Count < model.NumSlots)
			{
				item.Price = (int)((float)item.Price*model.SellbackMultiplier);
				model.Inventory.Add(item);
				model.Market.Remove(item);
				AmbitionApp.GetModel<GameModel>().Livre -= item.Price;
			}
		}
	}
}
