using System;
using Core;

namespace Ambition
{
	public class AdvanceDayCmd : ICommand
	{
		public void Execute()
		{
			CalendarModel cmod = AmbitionApp.GetModel<CalendarModel>();
			cmod.Today = cmod.Today.AddDays(1);
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			model.Party = Array.Find(model.Parties, p=>p.Date == cmod.Today && p.RSVP == 1);
		}
	}
}
