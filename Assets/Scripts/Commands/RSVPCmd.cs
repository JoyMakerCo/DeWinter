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

			if (party.Date == calendar.Today && party.RSVP == 1)
			{
				model.Party = party;
			}
			else if (model.Party == party && party.RSVP != 1)
	    	{
	    		model.Party = null;
	    	}
		}
	}
}
