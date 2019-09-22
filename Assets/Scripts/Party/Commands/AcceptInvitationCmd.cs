using System;
using System.Linq;
using Core;
namespace Ambition
{
    public class AcceptInvitationCmd : ICommand<PartyVO> 
    {
        public void Execute(PartyVO party)
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            if (party != null && party.Date >= calendar.Today)
            {
                PartyVO[] parties = calendar.GetEvents<PartyVO>(party.Date).Where(p=>p!=party).ToArray();
                if (party.RSVP != RSVP.Required && Array.Exists(parties, p => p.RSVP == RSVP.Required))
                {
                    AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, party);
                }
                else
                {
                    if (party.RSVP != RSVP.Required) party.RSVP = RSVP.Accepted;
                    // TODO: Any benefits for RSVP yes goes here
                    parties = parties.Where(p => p.RSVP != RSVP.Declined).ToArray();
                    Array.ForEach(parties, p => AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, p));
                    calendar.Schedule(party);
                }
            }
        }
    }
}
