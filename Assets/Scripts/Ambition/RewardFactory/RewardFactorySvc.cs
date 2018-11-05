using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
    public class RewardFactorySvc : IAppService
    {
        private Dictionary<CommodityType, Action<CommodityVO>> _rewardHandlers = new Dictionary<CommodityType, Action<CommodityVO>>();
        public void RegisterReward<T>(CommodityType type) where T:ICommand<CommodityVO>, new()
        {
            _rewardHandlers.Add(type, c => new T().Execute(c));
        }

        public void Reward(CommodityVO commodity)
        {
            // Will throw an exception
            _rewardHandlers[commodity.Type].Invoke(commodity);
        }
    }
}
