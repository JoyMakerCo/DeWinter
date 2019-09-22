using System;
using Core;

namespace Ambition
{
    // DEPRECATED
	public class FactionTurnModifierCmd : ICommand<PartyVO>
	{
		public void Execute (PartyVO party)
		{
/*			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			FactionVO faction = AmbitionApp.GetModel<FactionModel>()[party.Faction];

			//Extra Turns because of Faction Reputation Level
			switch (party.Size)
			{
                case PartySize.Trivial:
					if (faction.Level >= 4)
                        party.Turns += 2;
					break;
                case PartySize.Decent:
					if (faction.Level >= 7)
                        party.Turns += 3;
					break;
                case PartySize.Grand:
					if (faction.Level >= 9)
                        party.Turns += 4;
					break;
			}
*/
        }
	}
}
