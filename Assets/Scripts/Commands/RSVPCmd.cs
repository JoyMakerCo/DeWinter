using System;
using Core;

namespace DeWinter
{
	public class RSVPCmd : ICommand<Party>
	{
		public void Execute (Party party)
		{
			if (party.RSVP > 0) // Confirming RSVP
			{
				
			}
			else // Denying RSVP
			{
				int rep = 0;
//				int rep = (DeWinterApp.GetModel<CalendarModel>().Today == party.Date) ? 40 : 20;
				DeWinterApp.SendMessage<AdjustFactionVO>(new AdjustFactionVO(party.faction, -rep));
				DeWinterApp.AdjustValue<int>(GameConsts.REPUTATION, -rep);
			}
		}
	}
}