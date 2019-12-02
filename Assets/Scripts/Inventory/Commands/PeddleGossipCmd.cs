using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Ambition
{
	public class PeddleGossipCmd : ICommand<ItemVO>
	{
		public void Execute (ItemVO item)
		{
			if (item.Type != ItemType.Gossip)
			{
				Debug.LogError("PeddleGossipCmd invoked on non-gossip item: " + item.ToString() );
				return;
			}

			InventoryModel model = AmbitionApp.GetModel<InventoryModel>();
			if (model.Inventory.ContainsKey(item.Type) && model.Inventory[item.Type].Remove(item))
			{
				model.GossipShared(item);
				model.Broadcast();
			}
		}
	}
}
