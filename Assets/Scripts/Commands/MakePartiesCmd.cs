using System;
using Core;

namespace DeWinter
{
	public class MakePartiesCmd : ICommand<DateTime>
	{
		public void Execute (DateTime day)
		{
			CalendarModel cmod = DeWinterApp.GetModel<CalendarModel>();
			Random rnd = new Random();

			// A social event for today
			Party p =  new Party(new Random().Next(4));
			p.Date = day.Date;
			cmod.AddParty(p);

			if (rnd.Next(3) == 0) // Chance of a random future engagement
			{
				p = new Party(rnd.Next(4));
				p.Date = cmod.DaysFromNow(rnd.Next(2,6));
				cmod.AddParty(p);
			}
		}
	}
}