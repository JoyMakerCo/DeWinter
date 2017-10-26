using System;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
	public class CreateInvitationsState : UState
	{
		public override void OnEnterState ()
		{
			CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
			DateTime day = calendar.Today;
			AmbitionApp.SendMessage<DateTime>(day);
			Random rnd = new Random();

			// A social event for today
			PartyVO party =  new PartyVO(new Random().Next(4));
			party.Date = day.Date;
			party.invited = true;
			calendar.AddParty(party);

			if (rnd.Next(3) == 0) // Chance of a random future engagement
			{
				party = new PartyVO(rnd.Next(4));
				party.Date = calendar.DaysFromNow(rnd.Next(2,6));
				party.invited = true;
				calendar.AddParty(party);
			}
		}
	}
}
