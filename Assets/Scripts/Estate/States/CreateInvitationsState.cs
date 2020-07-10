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
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            OccasionVO[] occasions = calendar.GetOccasions(OccasionType.Party);
            foreach(OccasionVO occasion in occasions)
            {
                if (model.Parties.TryGetValue(occasion.ID, out PartyVO party) && party.RSVP == RSVP.New)
                {
                    // Display Invitations for new parties
                    // TODO: this needs a way better UI
                    AmbitionApp.OpenDialog(RSVPDialog.DIALOG_ID, party);
                }
            }
        }
	}
}
