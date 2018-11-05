using System;
namespace Ambition
{
    public class RepReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            FactionModel factions = AmbitionApp.GetModel<FactionModel>();
            if (reward.ID != null && factions.Factions.ContainsKey(reward.ID))
            {
                factions[reward.ID].Reputation += reward.Value;
                AmbitionApp.SendMessage(factions[reward.ID]);
            }
            else
            {
                AmbitionApp.GetModel<GameModel>().Reputation += reward.Value;
            }
        }
    }
}
