using System;
using System.Collections.Generic;
using UFlow;
using Core;

namespace Ambition
{
    public class FleeConversationState : UState
	{
		public override void OnEnterState ()
		{
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            PartyVO party = model.Party;

            // Lose a turn
            model.Turn++;

            //The Player is relocated to the Entrance
            MapModel mmod = AmbitionApp.GetModel<MapModel>();
            mmod.Room = mmod.Map.Entrance;

            //The Player's Reputation is Punished
            // TODO: Make these values configurable
            AmbitionApp.GetModel<GameModel>().Reputation -= 25;
            AmbitionApp.SendMessage<AdjustFactionVO>(new AdjustFactionVO(party.Faction, -50));

            AmbitionApp.SendMessage(PartyMessages.SHOW_MAP);
     	}
	}
}
