using System;
using Core;

namespace Ambition
{
	public class FactionTurnModifierCmd : ICommand<PartyVO>
	{
		public void Execute (PartyVO party)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			FactionVO faction = AmbitionApp.GetModel<FactionModel>()[party.Faction];

			//Extra Turns because of Faction Reputation Level
			switch (party.Importance)
			{
                case PartySize.Trivial:
					if (faction.Level >= 4)
                        model.Party.Turns += 2;
					break;
                case PartySize.Decent:
					if (faction.Level >= 7)
                        model.Party.Turns += 3;
					break;
                case PartySize.Grand:
					if (faction.Level >= 9)
                        model.Party.Turns += 4;
					break;
			}
		}
	}
}
