using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class SelectDateCmd : ICommand<DateTime>
	{
		public void Execute (DateTime date)
		{
			Dictionary<DateTime, List<PartyVO>> calendar = AmbitionApp.GetModel<CalendarModel>().Parties;
			if (calendar.ContainsKey(date))
			{
				List<PartyVO> parties = calendar[date].FindAll(x => x.invited);
				if (parties.Count == 0) return; // No parties!

				if (parties.Count == 1) // Easy choice.
				{
					if (parties[0].RSVP < 1)
					{
						AmbitionApp.OpenDialog<PartyVO>(DialogConsts.RSVP, parties[0]);
					}
					else
					{
						AmbitionApp.OpenDialog<PartyVO>(DialogConsts.CANCEL, parties[0]);
					}
				}
				else // Multiple Parties
				{
					AmbitionApp.OpenDialog<List<PartyVO>>(DialogConsts.RSVP_CHOICE, parties);
				}
			}
		}
	}
}