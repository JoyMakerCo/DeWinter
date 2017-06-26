using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class SelectDateCmd : ICommand<DateTime>
	{
		public void Execute (DateTime date)
		{
			Dictionary<DateTime, List<Party>> calendar = DeWinterApp.GetModel<CalendarModel>().Parties;
			if (calendar.ContainsKey(date))
			{
				List<Party> parties = calendar[date].FindAll(x => x.invited);
				if (parties.Count == 0) return; // No parties!

				if (parties.Count == 1) // Easy choice.
				{
					if (parties[0].RSVP < 1)
					{
						DeWinterApp.OpenDialog<Party>(DialogConsts.RSVP, parties[0]);
					}
					else
					{
						DeWinterApp.OpenDialog<Party>(DialogConsts.CANCEL, parties[0]);
					}
				}
				else // Multiple Parties
				{
					DeWinterApp.OpenDialog<List<Party>>(DialogConsts.RSVPx2, parties);
				}
			}
		}
	}
}