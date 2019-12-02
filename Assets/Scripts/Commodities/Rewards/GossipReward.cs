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
            InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
            if (reward.ID == null || !Enum.TryParse<FactionType>(reward.ID, ignoreCase:true, out FactionType faction))
            {
                faction = AmbitionApp.GetModel<PartyModel>().Party?.Faction ?? FactionType.Neutral;
            }

            var payload = new GossipRewardSpec();
            payload.Faction = faction;
            payload.Tier = reward.Value;
            AmbitionApp.SendMessage(InventoryMessages.CREATE_GOSSIP, payload);
        }
    }
}
