using System;
using System.Collections.Generic;

namespace Ambition
{
    public class PartyReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            PartyVO party = reward.ID != null ? model.LoadParty(reward.ID) : model.Party;
            if (party != null)
            {
                if (party != model.Party || model.Turn == 0)
                {
                    CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                    party.RSVP = (RSVP)reward.Value;
                    party.InvitationDate = calendar.Today;

                    if (default(DateTime).Equals(party.Date))
                        party.Date = calendar.Today;

                    calendar.Schedule(party);

                    if (party.RSVP == RSVP.Accepted && party.Date == calendar.Today)
                        model.Party = party;
                }
                else if (reward.Value < 0)
                {
                    party.Turns = model.Turn;
                }
            }
        }
    }
}
