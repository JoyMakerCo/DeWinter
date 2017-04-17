using System;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class SelectDateCmd : ICommand<DateTime>
	{
		public void Execute (DateTime date)
		{
			CalendarModel cmod = DeWinterApp.GetModel<CalendarModel>();
	        List<Party> parties;
			if (date != default(DateTime) && cmod.Parties.TryGetValue(date, out parties))
			{
				parties = parties.FindAll(x => x.invited);
				if (parties.Count == 0) return; // No parties!

				Party party = parties.Find(x => x.RSVP == 1);
				if (party != null) // You RSVP'd. Cancel?
				{
					DeWinterApp.OpenDialog<Party>(DialogConsts.CANCEL_RSVP_DIALOG, party);
				}
				else if (parties.Count == 1) // Easy choice.
				{
					DeWinterApp.OpenDialog<Party>(DialogConsts.RSVP_DIALOG, parties[0]);
				}
				else // Multiple Parties
				{
					DeWinterApp.OpenDialog<List<Party>>(DialogConsts.RSVP_CHOICE_DIALOG, parties);
				}
			}
		}
	}
}