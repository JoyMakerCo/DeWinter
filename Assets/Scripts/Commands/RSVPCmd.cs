using System;
using Core;

namespace Ambition
{
	public class RSVPCmd : ICommand<PartyVO>
	{
		public void Execute (PartyVO party)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();

			if (party.RSVP == -1)
			{
				int rep = (AmbitionApp.GetModel<CalendarModel>().Today == party.Date) ? 40 : 20;
				AmbitionApp.SendMessage<AdjustFactionVO>(new AdjustFactionVO(party.Faction, -rep));
				AmbitionApp.GetModel<GameModel>().Reputation -= rep;
			}
			AmbitionApp.SendMessage<PartyVO>(PartyMessages.PARTY_UPDATE, party);
			if (party.Date == calendar.Today)
			{
				model.Party = (party.RSVP == 1) ? party : null;
			}
		}
	}
}
