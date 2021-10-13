using System;
using System.Collections.Generic;
using Util;
namespace Ambition
{
    public class QuestReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO commodity)
        {
            GossipModel model = AmbitionApp.Gossip;
            if (model.Quests.Count >= model.MaxQuests) return;

            FactionModel factions = AmbitionApp.GetModel<FactionModel>();
            QuestVO quest = new QuestVO();
            CommodityVO[] rewards;
            int rewardTier = RNG.Generate(2) < 1 ? 0 : RNG.Generate(3) < 2 ? 1 : 2;

            if (!Enum.TryParse<FactionType>(commodity.ID, true, out quest.Faction))
            {
                List<FactionType> factionTypes = new List<FactionType>(factions.Factions.Keys);
                factionTypes.Remove(FactionType.None);
                quest.Faction = RNG.TakeRandom(factionTypes);
            }

            quest.Created = AmbitionApp.Calendar.Day;
            quest.Due = commodity.Value >= 5
                ? quest.Created + commodity.Value
                : quest.Created + RNG.Generate(5) + RNG.Generate(4) + RNG.Generate(4) + 5;

            rewards = model.RewardTiers[rewardTier].Rewards;
            quest.Reward = RNG.TakeRandom(rewards);
            AmbitionApp.Localization.SetCurrentQuest(quest);
            AmbitionApp.Gossip.Quests.Add(quest);
            AmbitionApp.Gossip.Broadcast();
        }
    }
}
