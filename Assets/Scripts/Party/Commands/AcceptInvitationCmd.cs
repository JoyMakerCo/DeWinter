using System;
using System.Collections.Generic;
using Core;
namespace Ambition
{
    public class AcceptInvitationCmd : ICommand<PartyVO> 
    {
        public void Execute(PartyVO party)
        {
            CalendarModel calendar = AmbitionApp.Calendar;
            if (party == null || party.Day < calendar.Day) return;

            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            List<PartyVO> parties = new List<PartyVO>(calendar.GetOccasions<PartyVO>(party.Day));
            RendezVO[] rendezs = calendar.GetOccasions<RendezVO>(party.Day);
            parties.Remove(party);
            if (party.RSVP == RSVP.Required || (!parties.Exists(p=>p.IsAttending) && !Array.Exists(rendezs, r => r.IsAttending)))
            {
                parties.ForEach(p => AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, p));
                Array.ForEach(rendezs, p => AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, p));
                if (party.RSVP != RSVP.Required) party.RSVP = RSVP.Accepted;
                if (party.RSVP != RSVP.Required && party.Created == calendar.Day)
                {
                    CommodityVO reward = new CommodityVO(CommodityType.Credibility, model.AcceptInvitationBonus);
                    AmbitionApp.SendMessage(reward);
                }
                AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, party); // Dispatches Broadcast()
            }
            else AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, party);
        }
    }
}
