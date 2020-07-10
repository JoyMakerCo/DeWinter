using UFlow;
using System.Collections.Generic;

namespace Ambition
{
    public class CheckMissedPartiesState : UState
    {
        public override void OnEnterState()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            GameModel model = AmbitionApp.GetModel<GameModel>();
            PartyModel partym = AmbitionApp.GetModel<PartyModel>();
            OccasionVO[] occasions = calendar.GetOccasions(OccasionType.Party, calendar.Day-1, true);
            foreach (OccasionVO occasion in occasions)
			{                    
                if (partym.Parties.TryGetValue(occasion.ID, out PartyVO party) && party.RSVP == RSVP.New)
                {
                    model.Reputation -= model.MissedPartyPenalty;
                    Dictionary<string, string> subs = new Dictionary<string, string>()
                    {{"$PARTYNAME",occasion.ID}};
                    AmbitionApp.OpenDialog(DialogConsts.MISSED_RSVP_DIALOG, subs);
                }
            }
        }
    }
}
