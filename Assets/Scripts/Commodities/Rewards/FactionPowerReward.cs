using System;
using Core;

using UnityEngine;

namespace Ambition
{
    public class FactionPowerReward : ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            //Debug.LogFormat( "FactionPowerReward.Execute, {0} {1}", reward.ID, reward.Value);

            FactionModel factions = AmbitionApp.GetModel<FactionModel>();
            if (Enum.TryParse(reward.ID, ignoreCase:true, out FactionType factionID))
            {
                AmbitionApp.SendMessage(FactionMessages.ADJUST_FACTION,AdjustFactionVO.MakePowerVO(factionID, reward.Value));
                return;
            }

            Debug.LogErrorFormat("Couldn't find faction {0} in FactionPowerReward",reward.ID);
        }
    }
}
