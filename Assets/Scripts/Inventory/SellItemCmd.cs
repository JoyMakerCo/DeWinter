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

			if (item.Type == ItemType.Gossip)
			{
				Debug.LogError("SellGossipCmd invoked on gossip item: " + item.ToString() );
				return;
			}

			if (model.Inventory.Remove(item))
			{
                // make it available to buy back
                model.Market?.Add(item);
                AmbitionApp.GetModel<GameModel>().Livre.Value += item.Price;
                model.Broadcast();
			}
		}
	}
}
