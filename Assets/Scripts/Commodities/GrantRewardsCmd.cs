using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
    public class GrantRewardsCmd : ICommand<CommodityVO[]>
    {
        public void Execute(CommodityVO[] Rewards)
        {
            if (Rewards != null)
            {
                Array.ForEach(Rewards, AmbitionApp.Reward);
                if (AmbitionApp.Game.Activity == ActivityType.Party)
                {
                    PartyModel party = AmbitionApp.GetModel<PartyModel>();
                    if (party.Rewards == null) party.Rewards = new List<CommodityVO>(Rewards);
                    else party.Rewards.AddRange(Rewards);
                }
            }
        }
    }
}
