using System;
using Core;
namespace Ambition
{
    public class DeclineInvitationCmd : ICommand<PartyVO> 
    {
        public void Execute(PartyVO party)
        {
            party.RSVP = RSVP.Declined;
            AmbitionApp.Calendar.Broadcast();
        }
    }
}
