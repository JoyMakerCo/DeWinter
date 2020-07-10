using System;
using Core;
namespace Ambition
{
    public class AcceptInvitationCmd : ICommand<PartyVO> 
    {
        public void Execute(PartyVO party)
        {
            if (party == null) return;

            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            GameModel game = AmbitionApp.GetModel<GameModel>();
            ushort day = game.Convert(party.Date);
            if (day >= game.Day)
            {
                if (party.RSVP != RSVP.Required) party.RSVP = RSVP.Accepted;
                AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, party);
                PartyVO[] parties = model.GetParties(day);
                int required = -1;

                for (int i=parties.Length-1; i>=0; --i)
                {
                    if (parties[i].RSVP == RSVP.Required)
                    {
                        required = i;
                        for (int j=parties.Length-1; j>i; --j)
                        {
                            AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, party);
                        }
                    }
                    if (party.RSVP != RSVP.Required) party.RSVP = RSVP.Accepted;
                }
            }
            model.UpdateParty();
        }
    }
}
