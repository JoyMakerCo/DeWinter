using System;
using System.Linq;
using Core;
namespace Ambition
{
    public class AcceptInvitationCmd : ICommand<PartyVO> 
    {
        public void Execute(PartyVO party)
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, party);
            if (party != null && party.Date >= calendar.Today)
            {
                PartyVO[] parties = model.GetParties(party.Date);
                if (party.RSVP != RSVP.Required && Array.Exists(parties, p => p.RSVP == RSVP.Required))
                {
                    AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, party.Name);
                }
                else
                {
                    if (party.RSVP != RSVP.Required) party.RSVP = RSVP.Accepted;
                    // TODO: Any benefits for RSVP yes goes here
                    foreach(PartyVO p in parties)
                    {
                        if (p != party)
                        {
                            AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, p.Name);
                        }
                    }
                }
            }
        }
    }
}
