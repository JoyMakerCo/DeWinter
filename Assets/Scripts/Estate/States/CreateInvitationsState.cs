using System;
using System.Linq;
using System.Collections.Generic;
using UFlow;
using Core;

namespace Ambition
{
	public class CreateInvitationsState : UState
	{
		public override void OnEnterState ()
		{
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            DateTime today = calendar.Today;
            List<PartyVO> parties = calendar.GetEvents<PartyVO>(today).Where(p=>p.RSVP == RSVP.New).ToList();
			if (Util.RNG.Generate(0,3) == 0) // Chance of a random future engagement
			{
                PartyVO party = new PartyVO
                {
                    InvitationDate = today,
                    Date = today.AddDays(Util.RNG.Generate(1, 8) + Util.RNG.Generate(1, 8)), // +2d8 days
                    RSVP = RSVP.New
                };
                AmbitionApp.Execute<InitPartyCmd, PartyVO>(party);
                parties.Add(party);
            }
            parties.ForEach(p => AmbitionApp.OpenDialog(RSVPDialog.DIALOG_ID, p));
        }
	}
}
