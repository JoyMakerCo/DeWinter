using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Ambition
{
    public class CreateGossipCmd : ICommand<GossipVO>
    {
        public void Execute(GossipVO gossip)
        {
            gossip.IsPower = (gossip.Faction == FactionType.Crown || gossip.Faction == FactionType.Revolution || 0 == Util.RNG.Generate(0, 2));
            gossip.Created = AmbitionApp.GetModel<CalendarModel>().Day;
            AmbitionApp.Gossip.Gossip.Add(gossip);
            AmbitionApp.Gossip.Broadcast();
        }
    }
}
