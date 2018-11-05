using System;
using Core;

namespace Ambition
{
    public class GrantRewardsCmd : ICommand<CommodityVO[]>
    {
        public void Execute(CommodityVO[] Rewards)
        {
            if (Rewards != null)
            {
                //Array.ForEach(Rewards, AmbitionApp.Reward);
                foreach (CommodityVO c in Rewards)
                {
                    AmbitionApp.Reward(c);
                }
            } 


        }
    }
}
