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
            if (model.Market != null && model.Market.TryGetValue(item.Type, out List<ItemVO> list) && list.Remove(item))
            {
                if (!model.Inventory.ContainsKey(item.Type))
                    model.Inventory.Add(item.Type, new List<ItemVO>());
                model.Inventory[item.Type].Add(item);
                if (item.Created == default)
                {
                    item.Price = (int)(item.Price * model.SellbackMultiplier);
                    item.Created = AmbitionApp.GetModel<CalendarModel>().Today;
                }
                AmbitionApp.GetModel<GameModel>().Livre.Value -= item.Price;
                model.Broadcast();
            }
		}
	}
}
