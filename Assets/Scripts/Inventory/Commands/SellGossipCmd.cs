using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Ambition
{
	public class SellGossipCmd : ICommand<ItemVO>
	{
		public void Execute (ItemVO item)
		{
			if (item.Type != ItemType.Gossip)
			{
				Debug.LogError("SellGossipCmd invoked on non-gossip item: " + item.ToString() );
				return;
			}

			InventoryModel model = AmbitionApp.GetModel<InventoryModel>();
			if (model.Inventory.Remove(item))
			{
                AmbitionApp.GetModel<GameModel>().Livre.Value += GossipWrapperVO.GetValue(item);
				model.GossipShared(item);
				model.Broadcast();
			}
		}
	}
}
