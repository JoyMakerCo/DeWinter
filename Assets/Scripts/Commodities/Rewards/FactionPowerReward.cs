using System;
using Core;

using UnityEngine;

namespace Ambition
{
    public class FactionPowerReward : ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            if (Enum.TryParse(reward.ID, ignoreCase:true, out FactionType factionID))
            {
                AmbitionApp.Politics.Factions.TryGetValue(factionID, out FactionVO faction);
                if (faction != null)
                {
                    faction.Power += reward.Value;
                    if (faction.Power < 0) faction.Power = 0;
                    AmbitionApp.SendMessage(faction);
                    AmbitionApp.Politics.Broadcast();
                }
            }
        }
    }
}
