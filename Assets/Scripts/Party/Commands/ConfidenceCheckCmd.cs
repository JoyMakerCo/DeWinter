using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class ConfidenceCheckCmd : ICommand<int>
	{
		public void Execute (int confidence)
		{
			if (confidence <= 0)
			{
				PartyModel model = AmbitionApp.GetModel<PartyModel>();
				PartyVO party = model.Party;

				// Lose a turn
				model.TurnsLeft--;
				model.Confidence = (model.StartConfidence>>1);

	            //The Player is relocated to the Entrance
				MapModel mmod = AmbitionApp.GetModel<MapModel>();
	            mmod.Room = mmod.Map.Entrance;

				//The Player's Reputation is Punished
				// TODO: Make these values configurable
				AmbitionApp.GetModel<GameModel>().Reputation -= 25;
				AmbitionApp.SendMessage<AdjustFactionVO>(new AdjustFactionVO(party.Faction, -50));

	            //Explanation Screen Pop Up goes here
	            Dictionary<string, string> subs = new Dictionary<string, string>() {
	            {"$FACTION", AmbitionApp.GetString(party.Faction)},
				{"$REPUTATION", "25"},
				{"$FACTIONREPUTATION", "50"}};
				AmbitionApp.OpenMessageDialog("out_of_confidence_dialog", subs);

	            //The Player is pulled from the Work the Room session
				AmbitionApp.CloseDialog(DialogConsts.ROOM);
				AmbitionApp.CloseDialog(DialogConsts.HOST_ENCOUNTER);
     		}
     	}
	}
}