using System;
using System.Linq;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class CreateInvitationsState : UFlow.UState
	{
        public override void OnEnterState()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            List<PartyVO> parties = calendar.GetEvents<PartyVO>().Where(p => p.RSVP == RSVP.New).ToList();
            // Display Invitations for new parties
            // TODO: this needs a way better UI
            parties.ForEach(p => AmbitionApp.OpenDialog(RSVPDialog.DIALOG_ID, p));
        }
	}
}
