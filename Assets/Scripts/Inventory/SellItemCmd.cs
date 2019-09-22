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
			if (model.Inventory.ContainsKey(item.Type) && model.Inventory[item.Type].Remove(item))
			{
                AmbitionApp.GetModel<GameModel>().Livre.Value += item.Price;
                if (model.Market != null)
                {
                    // Buyback price will be the same, to the user can change her mind to her heart's content
                    if (!model.Market.ContainsKey(item.Type))
                        model.Market.Add(item.Type, new List<ItemVO>());
                    model.Market[item.Type].Add(item);
                }
                model.Broadcast();
			}
		}
	}
}
