using System;
using Core;

namespace Ambition
{
	public class RSVPCmd : ICommand<PartyVO>
	{
		public void Execute (PartyVO party)
		{
			if (party.RSVP > 0) // Confirming RSVP
			{
				
			}
			else // Denying RSVP
			{
				int rep = 0;
//				int rep = (AmbitionApp.GetModel<CalendarModel>().Today == party.Date) ? 40 : 20;
				AmbitionApp.SendMessage<AdjustFactionVO>(new AdjustFactionVO(party.Faction, -rep));
				AmbitionApp.GetModel<GameModel>().Reputation -= rep;
			}
		}
	}
}