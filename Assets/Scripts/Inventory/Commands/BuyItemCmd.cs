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
            if (model.Market?.Remove(item) ?? false)
            {
                Debug.Log("Found item in market and successfully removed it from market");
                model.Inventory.Add(item);

                Debug.LogFormat("Added {0} to player inventory",item.ToString());
                int cost = item.Price;
                if (item.Created == default)
                {
                    item.Price = (int)(cost * model.SellbackMultiplier);
                    item.Created = AmbitionApp.GetModel<GameModel>().Date;
                }
                AmbitionApp.GetModel<GameModel>().Livre.Value -= cost;
                model.Broadcast();
            }
		}
	}
}
