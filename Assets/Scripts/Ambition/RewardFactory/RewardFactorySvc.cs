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
#if DEBUG
            if (IsRegistered(type))
                UnityEngine.Debug.LogError(">> Attempting to register previously registered reward type: \"" + type + "\"");
            else
#endif
                _rewardHandlers[type] = c => new T().Execute(c);
        }

        public bool IsRegistered(CommodityType type) => _rewardHandlers.ContainsKey(type);

        public void Reward(CommodityVO commodity)
        {
            if (_rewardHandlers.TryGetValue(commodity.Type, out Action<CommodityVO> del))
                del(commodity);
#if DEBUG
            else UnityEngine.Debug.LogError(">> Reward Factory error: \"" + commodity.Type + "\" is not a known reward type");
#endif
        }

        public void Dispose() => _rewardHandlers.Clear();
    }
}
