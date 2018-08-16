using System;
using Core;

namespace Ambition
{
    public class GrantRewardsCmd : ICommand<CommodityVO[]>
    {
        public void Execute(CommodityVO[] Rewards)
        {
            Array.ForEach(Rewards, AmbitionApp.Execute<GrantRewardCmd, CommodityVO>);
        }
    }
}
