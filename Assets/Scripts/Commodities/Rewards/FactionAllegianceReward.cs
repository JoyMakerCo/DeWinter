using System;
using Core;

using UnityEngine;

namespace Ambition
{
    public class FactionAllegianceReward : ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            if (Enum.TryParse(reward.ID, ignoreCase: true, out FactionType factionID)
                && AmbitionApp.Politics.Factions.TryGetValue(factionID, out FactionVO faction)
                && faction != null
                && !faction.Steadfast)
            {
                faction.Allegiance += reward.Value;
                if (faction.Allegiance > 100) faction.Allegiance = 100;
                else if (faction.Allegiance < -100) faction.Allegiance = -100;
                AmbitionApp.SendMessage(faction);
                AmbitionApp.Politics.Broadcast();
            }
        }
    }
}
