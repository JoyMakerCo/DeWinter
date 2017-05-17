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
				PartyModel pmod = AmbitionApp.GetModel<PartyModel>();
				PartyVO party = pmod.Party;

				// Lose a turn
				AmbitionApp.AdjustValue<int>(PartyConstants.TURNSLEFT, -1);

				AmbitionApp.AdjustValue(GameConsts.CONFIDENCE, (int)(pmod.StartConfidence-confidence)*0.5f);

	            //The Player is relocated to the Entrance
				MapModel mmod = AmbitionApp.GetModel<MapModel>();
	            mmod.Room = mmod.Map.Entrance;

				//The Player's Reputation is Punished
				// TODO: Make these values configurable
				AmbitionApp.AdjustValue<int>(GameConsts.REPUTATION, -25);
				AmbitionApp.SendMessage<AdjustFactionVO>(new AdjustFactionVO(party.faction, -50));

	            //Explanation Screen Pop Up goes here
	            Dictionary<string, string> subs = new Dictionary<string, string>() {
	            {"$FACTION", party.faction},
				{"$REPUTATION", "25"},
				{"$FACTIONREPUTATION", "50"}};
				AmbitionApp.OpenMessageDialog("out_of_confidence_dialog", subs);

	            //The Player is pulled from the Work the Room session
				AmbitionApp.CloseDialog(DialogConsts.ENCOUNTER);
				AmbitionApp.CloseDialog(DialogConsts.HOST_ENCOUNTER);
     		}
     	}
	}
}