using System;
using System.Collections.Generic;

namespace Ambition
{
    public class PartyReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            if (string.IsNullOrEmpty(reward?.ID)) return;
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            PartyVO party = model.LoadParty(reward.ID);

            if (party == null) return;

            switch(reward.Value)
            {
                case -1:
                    party.RSVP = RSVP.Declined;
                    AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, party);
                    AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, party);
                    break;
                case 1:
                    party.RSVP = RSVP.Accepted;
                    AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, party);
                    AmbitionApp.SendMessage(PartyMessages.ACCEPTED, party);
                    break;
                case 2:
                    party.RSVP = RSVP.Required;
                    AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, party);
                    AmbitionApp.SendMessage(PartyMessages.ACCEPTED, party);
                    break;
            }
        }
    }
}
