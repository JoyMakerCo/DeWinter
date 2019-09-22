using System;
using Core;

using UnityEngine;

namespace Ambition
{
    public class FactionAllegianceReward : ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            //Debug.LogFormat( "FactionAllegianceReward.Execute, {0} {1}", reward.ID, reward.Value);
            FactionModel factions = AmbitionApp.GetModel<FactionModel>();
            if (Enum.TryParse(reward.ID, out FactionType factionID))
            {
                AmbitionApp.SendMessage(FactionMessages.ADJUST_FACTION,AdjustFactionVO.MakeAllegianceVO(factionID, reward.Value));
                return;
            }

            Debug.LogErrorFormat("Couldn't find faction {0} in FactionPowerReward",reward.ID);
        }
    }
}
