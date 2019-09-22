using System;
using System.Collections.Generic;

namespace Ambition
{
    public class PartyReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            if (reward.ID != null)
            {
                PartyConfig config = UnityEngine.Resources.Load<PartyConfig>("Parties/" + reward.ID);
                PartyVO party = config?.GetParty();
                if (party != null)
                {
                    CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                    party.RSVP = (RSVP)reward.Value;
                    party.InvitationDate = calendar.Today;
                    party.IntroIncident = config.IntroIncident?.GetIncident();
                    party.ExitIncident = config.ExitIncident?.GetIncident();
                    AmbitionApp.GetModel<MapModel>().SaveMap(party, config.Map);

                    if (party.Date == default) party.Date = calendar.Today;
                    AmbitionApp.SendMessage(PartyMessages.INITIALIZE_PARTY, party);

                    switch (party.RSVP)
                    {
                        case RSVP.Accepted:
                        case RSVP.Required:
                            AmbitionApp.SendMessage(PartyMessages.ACCEPT_INVITATION, party);
                            break;
                        case RSVP.Declined:
                            AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, party);
                            break;
                    }
                }
                else
                {
                    UnityEngine.Debug.Log("Warning: PartyReward.cs: No party with ID \"" + reward.ID + "\" exists!");
                }
                config = null;
                UnityEngine.Resources.UnloadUnusedAssets();
            }
            else
                UnityEngine.Debug.Log("Warning: PartyReward.cs: No party ID specified!");

        }
    }
}
