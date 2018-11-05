using System;
using UFlow;

namespace Ambition
{
    public class LoadParisIncidentState : UState
    {
        public override void OnEnterState()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();
            calendar.Incident = paris.Location.IntroIncidentConfig.Incident;
        }
    }
}
