using System;
using Core;
namespace Ambition
{
    public class DeclineInvitationCmd : ICommand<PartyVO> 
    {
        public void Execute(PartyVO party)
        {
            // You can't decline a required party.
            if (party.RSVP != RSVP.Required)
            {
                CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                party.RSVP = RSVP.Declined;
                calendar.Schedule(party);
                // TODO: Any penalties for RSVP No goes here
            }
        }
    }
}
