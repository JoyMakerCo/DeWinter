using System;
using Core;

namespace Ambition
{
    public class TutorialFleeConversationCmd : ICommand
    {
        public void Execute()
        {
/*DEPRECATED            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            CommodityVO[] penalties = {
                new CommodityVO(CommodityType.Reputation, null, -model.FleePartyPenalty),
                new CommodityVO(CommodityType.Reputation, model.Party.Faction, -model.FleeFactionPenalty)
            };
            AmbitionApp.SendMessage(PartyMessages.FLEE_PENALTIES, penalties);
            model.DeckSize = 20;

            MapModel map = AmbitionApp.GetModel<MapModel>();
            AmbitionApp.SendMessage(MapMessage.GO_TO_ROOM, map.Map.Entrance);
 */       }
    }
}
