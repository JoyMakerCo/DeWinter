using System;
namespace Ambition
{
    public class RepReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            FactionModel factions = AmbitionApp.GetModel<FactionModel>();
            if (reward.ID != null
                && Enum.TryParse<FactionType>(reward.ID, out FactionType factionType)
                && factions.Factions.TryGetValue(factionType, out FactionVO faction))
            {
                faction.Reputation += reward.Value;
                AmbitionApp.SendMessage(faction);
            }
            else AmbitionApp.GetModel<GameModel>().Reputation += reward.Value;
        }
    }
}
