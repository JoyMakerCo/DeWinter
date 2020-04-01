using System;
namespace Ambition
{
    public class RepReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            FactionModel factions = AmbitionApp.GetModel<FactionModel>();
            if (reward.ID != null
                && Enum.TryParse<FactionType>(reward.ID, ignoreCase:true, out FactionType factionType))
            {
                AmbitionApp.SendMessage(FactionMessages.ADJUST_FACTION,AdjustFactionVO.MakeReputationVO(factionType, reward.Value));
            }
            else AmbitionApp.GetModel<GameModel>().Reputation += reward.Value;
        }
    }
}

