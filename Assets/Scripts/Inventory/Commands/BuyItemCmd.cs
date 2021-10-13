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
            InventoryModel inventory = AmbitionApp.Inventory;
            if (item != null
                && AmbitionApp.Game.Livre >= item.Price
                && inventory.Market != null
                && inventory.Market.Remove(item))
            {
                CommodityVO cost = new CommodityVO(CommodityType.Livre, -item.Price);
                AmbitionApp.SendMessage(cost);
                if (item.Created == default)
                {
                    item.Created = AmbitionApp.GetModel<CalendarModel>().Today;
                }
                item.Price = (int)(item.Price * inventory.SellbackMultiplier);
                inventory.Inventory.Add(item);
                inventory.Broadcast();
            }
		}
	}
}
