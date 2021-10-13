using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Ambition
{
	public class SellGossipCmd : ICommand<GossipVO>
	{
		public void Execute (GossipVO gossip)
		{
			if (gossip != null)
            {
                int value = AmbitionApp.Gossip.GetValue(gossip, AmbitionApp.Calendar.Day);
                CommodityVO profit = new CommodityVO(CommodityType.Livre, value);
                AmbitionApp.SendMessage(profit);
                AmbitionApp.Gossip.Gossip.Remove(gossip);
                ++AmbitionApp.Gossip.GossipActivity;
                AmbitionApp.Gossip.Broadcast();
            }
		}
	}
}
