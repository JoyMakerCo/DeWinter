using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class UpdatePartyCmd : ICommand<PartyVO>
	{
		public void Execute (PartyVO party)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            if (!calendar.Timeline.ContainsKey(party.Date))
                calendar.Timeline.Add(party.Date, new List<ICalendarEvent>());

            if (!calendar.Timeline[party.Date].Contains(party))
                calendar.Timeline[party.Date].Add(party);

            // Cancelled Party!
            if (party.RSVP == RSVP.Declined)
            {
                int rep = (AmbitionApp.GetModel<CalendarModel>().Today == party.Date) ? 40 : 20;
                AmbitionApp.SendMessage(new AdjustFactionVO(party.Faction, -rep));
                AmbitionApp.GetModel<GameModel>().Reputation -= rep;
            }

            // Update today's Party
            if (party.Date == calendar.Today && party.RSVP == RSVP.Accepted)
			{
                // Cancel previously accepted party!
                if (model.Party != null && model.Party != party)
                {
                    model.Party.RSVP = RSVP.Declined;
                    AmbitionApp.SendMessage(model.Party);
                }
                model.Party = party;
			}
		}
	}
}
