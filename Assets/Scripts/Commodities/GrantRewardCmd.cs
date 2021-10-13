using Core;
using System.Collections.Generic;

namespace Ambition
{
	public class GrantRewardCmd : ICommand<CommodityVO>
	{
		public void Execute(CommodityVO reward)
		{
            AmbitionApp.Reward(reward);
            if (AmbitionApp.Game.Activity == ActivityType.Party)
            {
                PartyModel party = AmbitionApp.GetModel<PartyModel>();
                if (party.Rewards == null) party.Rewards = new List<CommodityVO>() { reward };
                else party.Rewards.Add(reward);
            }
        }
    }
}
