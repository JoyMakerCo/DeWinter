using System;
using Core;

namespace DeWinter
{
	public class CreateInvitationsCmd : ICommand<DateTime>
	{
		public void Execute (DateTime day)
		{
			CalendarModel cmod = DeWinterApp.GetModel<CalendarModel>();
			Random rnd = new Random();

			// A social event for today
			Party p =  new Party(new Random().Next(4));
			p.Date = day.Date;
			p.invited = true;
			cmod.UpdateParty(p);
			DeWinterApp.OpenDialog<Party>(RSVPDialogMediator.DIALOG_ID, p);

			if (rnd.Next(3) == 0) // Chance of a random future engagement
			{
				p = new Party(rnd.Next(4));
				p.Date = cmod.DaysFromNow(rnd.Next(2,6));
				p.invited = true;
				cmod.UpdateParty(p);
				DeWinterApp.OpenDialog<Party>(RSVPDialogMediator.DIALOG_ID, p);
			}
		}
	}
}