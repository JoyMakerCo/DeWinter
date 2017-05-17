using System;
using Core;

namespace Ambition
{
	public class CreateInvitationsCmd : ICommand<DateTime>
	{
		public void Execute (DateTime day)
		{
			CalendarModel cmod = AmbitionApp.GetModel<CalendarModel>();
			Random rnd = new Random();

			// A social event for today
			PartyVO p =  new PartyVO(new Random().Next(4));
			p.Date = day.Date;
			p.invited = true;
			cmod.UpdateParty(p);
			AmbitionApp.OpenDialog<PartyVO>(DialogConsts.RSVP, p);

			if (rnd.Next(3) == 0) // Chance of a random future engagement
			{
				p = new PartyVO(rnd.Next(4));
				p.Date = cmod.DaysFromNow(rnd.Next(2,6));
				p.invited = true;
				cmod.UpdateParty(p);
				AmbitionApp.OpenDialog<PartyVO>(DialogConsts.RSVP, p);
			}
		}
	}
}