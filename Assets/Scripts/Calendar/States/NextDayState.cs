using System;
using System.Linq;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
    public class NextDayState : UState
    {
        public override void OnEnterState()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            calendar.Today = calendar.Today.AddDays(1);
            AmbitionApp.GetModel<PartyModel>().Party = null;
        }
    }
}
