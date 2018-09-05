using System;
using UFlow;

namespace Ambition
{
    public class NextDayState : UState
    {
        public override void OnEnterState()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            calendar.Today = calendar.Today.AddDays(1);
            PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
            System.Collections.Generic.List<PartyVO> parties;
            partyModel.Party =
                calendar.Parties.TryGetValue(calendar.Today, out parties)
                ? parties.Find(p => p.RSVP == 1)
                : null;
        }
    }
}
