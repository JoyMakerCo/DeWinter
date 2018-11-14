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
                if (config != null && config.Party != null)
                {
                    CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                    PartyVO party = config.Party;
                    AmbitionApp.Execute<InitPartyCmd, PartyVO>(party);
                    party.RSVP = (RSVP)reward.Value;
                    party.InvitationDate = calendar.Today;

                    if (default(DateTime) == party.Date)
                        party.Date = calendar.Today;

                    calendar.Schedule(party);

                    if (party.RSVP == RSVP.Accepted && party.Date == calendar.Today)
                        AmbitionApp.GetModel<PartyModel>().Party = party;
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
