using System;
using System.Linq;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class SelectDateCmd : ICommand<DateTime>
	{
		public void Execute (DateTime date)
		{
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            PartyVO[] parties = calendar.GetEvents<PartyVO>(date);
            switch (parties.Length)
            {
                case 0:
                    break; // Don't do shit.
                case 1: // Decide whether you're staying or going.
                    AmbitionApp.OpenDialog(parties[0].RSVP == RSVP.Accepted
                                           ? DialogConsts.CANCEL
                                           : DialogConsts.RSVP, parties[0]);
                    break;

                // You must choose
                // But choose wisely
                // For while the true Party grants eternal life
                // A false one will take it from you
                default:
                    AmbitionApp.OpenDialog(DialogConsts.RSVP_CHOICE, parties);
                    break;
            }
		}
	}
}