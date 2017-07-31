using System;
using Core;

namespace Ambition
{
	public class FactionTurnModifierCmd : ICommand<PartyVO>
	{
		public void Execute (PartyVO party)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			FactionVO faction = AmbitionApp.GetModel<FactionModel>().Factions[party.Faction];
			int turnsMod = 0;

			//Extra Turns because of Faction Reputation Level?
			switch (party.partySize)
			{
				case 1:
					if (faction.ReputationLevel >= 4)
						turnsMod = 2;
					break;
				case 2:
					if (faction.ReputationLevel >= 7)
						turnsMod = 3;
					break;
				case 3:
					if (faction.ReputationLevel >= 9)
						turnsMod = 4;
					break;
			}
			model.Party.Turns += turnsMod;
			AmbitionApp.AdjustValue<int>(PartyConstants.TURNSLEFT, turnsMod);
		}
	}
}