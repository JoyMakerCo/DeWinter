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
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            OccasionVO[] occasions = calendar.GetOccasions(OccasionType.Party, date.Subtract(calendar.StartDate).Days);
            PartyVO party;
            switch (occasions.Length)
            {
                case 0:
                    break; // Don't do shit.
                case 1: // Decide whether you're staying or going.
                    if (model.Parties.TryGetValue(occasions[0].ID, out party))
                    {
                        AmbitionApp.OpenDialog(party.RSVP == RSVP.Accepted
                                               ? DialogConsts.CANCEL
                                               : DialogConsts.RSVP, occasions[0]);
                    }
                    break;

                // You must choose
                // But choose wisely
                // For while the true Party grants eternal life
                // A false one will take it from you
                default:
                    PartyVO[] parties = new PartyVO[occasions.Length];
                    for (int i=parties.Length-1; i>=0; --i)
                    {
                        model.Parties.TryGetValue(occasions[i].ID, out party);
                        parties[i] = party;
                    }
                    AmbitionApp.OpenDialog(DialogConsts.RSVP_CHOICE, occasions);
                    break;
            }
		}
	}
}