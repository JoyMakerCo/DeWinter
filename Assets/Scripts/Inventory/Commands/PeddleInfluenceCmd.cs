using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Ambition
{
	public class PeddleInfluenceCmd : ICommand<GossipVO>
	{
		public void Execute (GossipVO gossip)
		{
            if (AmbitionApp.Gossip.Gossip.Remove(gossip))
            {
                ++AmbitionApp.Gossip.GossipActivity;
                AmbitionApp.Gossip.Broadcast();
            }
        }
	}
}
