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
            InventoryModel inventory = AmbitionApp.Inventory;
            int livre = AmbitionApp.Game.Livre;
			if (item != null && livre + item.Price >= 0 && inventory.Inventory.Remove(item))
			{
                CommodityVO profit = new CommodityVO(CommodityType.Livre, item.Price);
                AmbitionApp.SendMessage(profit);
                // Seller will markup the used price.
                item.Price = (int)(item.Price / inventory.SellbackMultiplier);
                inventory.Market?.Add(item);
                inventory.Broadcast();
			}
		}
	}
}
