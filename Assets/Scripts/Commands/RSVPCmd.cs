using System;
using Core;

namespace Ambition
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
				DeWinterApp.SendMessage<AdjustValueVO>(new AdjustValueVO(party.faction, -rep));
	            GameData.reputationCount -= rep;
			}
		}
	}
}