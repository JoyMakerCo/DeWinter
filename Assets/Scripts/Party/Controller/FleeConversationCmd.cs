namespace Ambition
{
    public class FleeConversationCmd : Core.ICommand
    {
        public void Execute()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            CommodityVO[] commodity = {
                new CommodityVO(CommodityType.Reputation, null, -model.FleePartyPenalty),
                new CommodityVO(CommodityType.Reputation, model.Party.Faction, -model.FleeFactionPenalty)
            };
            AmbitionApp.SendMessage(commodity);
        }
    }
}
