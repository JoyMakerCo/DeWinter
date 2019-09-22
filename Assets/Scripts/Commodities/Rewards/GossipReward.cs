using System;
using System.Collections.Generic;
namespace Ambition
{
    public class GossipReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
            if (reward.ID == null || !Enum.TryParse<FactionType>(reward.ID, out FactionType faction))
            {
                faction = AmbitionApp.GetModel<PartyModel>().Party?.Faction ?? FactionType.Neutral;
            }
            AmbitionApp.SendMessage(InventoryMessages.CREATE_GOSSIP, faction);
        }
    }
}
