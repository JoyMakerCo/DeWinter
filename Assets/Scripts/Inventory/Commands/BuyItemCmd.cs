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
            Debug.Log("BuyItemCmd.Execute");
			InventoryModel model = AmbitionApp.GetModel<InventoryModel>();
            if (model.Market != null && model.Market.TryGetValue(item.Type, out List<ItemVO> list) && list.Remove(item))
            {
                Debug.Log("Found item in market and successfully removed it from market");
                if (!model.Inventory.ContainsKey(item.Type))
                    model.Inventory.Add(item.Type, new List<ItemVO>());

                Debug.LogFormat("Added {0} to player inventory",item.ToString());
                model.Inventory[item.Type].Add(item);

                var cost = item.Price;
                if (item.Created == default)
                {
                    item.Price = (int)(item.Price * model.SellbackMultiplier);
                    item.Created = AmbitionApp.GetModel<CalendarModel>().Today;
                }
                AmbitionApp.GetModel<GameModel>().Livre.Value -= cost;
                model.Broadcast();
            }
		}
	}
}
