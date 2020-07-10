using System;
namespace Ambition
{
    public class PartyRewardCmd : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO commodity)
        {
            AmbitionApp.GetModel<PartyModel>().Party?.Rewards?.Add(commodity);
        }
    }
    public class PartyRewardsCmd : Core.ICommand<CommodityVO[]>
    {
        public void Execute(CommodityVO[] commodities)
        {
            AmbitionApp.GetModel<PartyModel>().Party?.Rewards?.AddRange(commodities);
        }
    }

}
