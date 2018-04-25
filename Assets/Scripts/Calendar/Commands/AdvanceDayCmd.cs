using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class AdvanceDayCmd : ICommand
	{
		public void Execute()
		{
			CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
			calendar.Today = calendar.Today.AddDays(1);
            PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
            List<PartyVO> parties;
			partyModel.Party = 
				calendar.Parties.TryGetValue(calendar.Today, out parties)
				? parties.Find(p => p.RSVP == 1)
				: null;
		}
	}
}
