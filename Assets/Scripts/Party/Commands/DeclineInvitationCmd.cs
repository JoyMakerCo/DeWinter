using System;
using Core;
namespace Ambition
{
    public class DeclineInvitationCmd : ICommand<PartyVO> 
    {
        public void Execute(PartyVO party)
        {
            if (party == null || party.RSVP == RSVP.Required) return;
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            GameModel game = AmbitionApp.GetModel<GameModel>();
            party.RSVP = RSVP.Declined;
            AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, party);
            AmbitionApp.SendMessage(PartyMessages.DECLINED, party);
        }
    }
}
