using System;
using Core;

namespace Ambition
{
	public class GenerateRandomPartyRewardCmd : ICommand<int>
	{
		public void Execute (int level)
		{
			PartyVO party = AmbitionApp.GetModel<PartyModel>().Party;
			RewardVO reward;
			switch (new Random().Next(5))
			{
				case 0:
				case 1:
					reward = new RewardVO(RewardConsts.VALUE, GameConsts.REPUTATION, 5*level);
					break;
				case 2:
				case 3:
					reward = new RewardVO(RewardConsts.FACTION, party.faction, 10*level);
					break;
				default:
					reward = (level < 5)
						? new RewardVO(RewardConsts.GOSSIP, party.faction, 1)
						: new RewardVO(RewardConsts.SERVANT, ServantConsts.SEAMSTRESS, 1);
					break;
			}
			AmbitionApp.GetModel<PartyModel>().Rewards.Add(reward);
		}
	}
}