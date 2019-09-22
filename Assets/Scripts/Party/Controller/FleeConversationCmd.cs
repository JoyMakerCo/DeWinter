namespace Ambition
{
    public class FleeConversationCmd : Core.ICommand
    {
        public void Execute()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            CommodityVO [] penalties = {
                new CommodityVO(CommodityType.Reputation, null, -model.FleePartyPenalty),
                new CommodityVO(CommodityType.Reputation, model.Party.Faction.ToString(), -model.FleeFactionPenalty)
            };
            AmbitionApp.SendMessage(PartyMessages.FLEE_PENALTIES, penalties);

            //MapModel map = AmbitionApp.GetModel<MapModel>();
            //AmbitionApp.SendMessage(MapMessage.GO_TO_ROOM, map.Map.Entrance);
        }
    }
}
