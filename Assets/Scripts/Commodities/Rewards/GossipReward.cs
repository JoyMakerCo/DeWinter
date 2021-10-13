using System;
using System.Collections.Generic;
namespace Ambition
{
    public class GossipRewardSpec
    {
        public FactionType Faction;
        public int Tier;
    }
    public class GossipReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            FactionType faction;
            if (reward.ID == null || !Enum.TryParse<FactionType>(reward.ID, ignoreCase: true, out faction))
            {
                faction = AmbitionApp.GetModel<PartyModel>().Party?.Faction ?? FactionType.None;
            }
            GossipVO gossip = new GossipVO()
            {
                Faction = faction,
                Tier = reward.Value
            };
            AmbitionApp.SendMessage(InventoryMessages.CREATE_GOSSIP, gossip);
        }
    }
}
