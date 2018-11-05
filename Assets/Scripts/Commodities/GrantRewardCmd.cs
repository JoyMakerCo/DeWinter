using Core;
using System;

namespace Ambition
{
	public class GrantRewardCmd : ICommand<CommodityVO>
	{
		public void Execute(CommodityVO reward)
		{
            AmbitionApp.Reward(reward);
		}
	}
}
