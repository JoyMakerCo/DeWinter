using System;
namespace Ambition
{
    public class GossipReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
            if (reward.ID != null) imod.GossipItems.Add(new Gossip(reward.ID));
            else
            {
                PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
                if (partyModel.Party != null) imod.GossipItems.Add(new Gossip(partyModel.Party.Faction));
            }
        }
    }
}
