﻿namespace Ambition
{
    public class LoadParisIncidentState : UFlow.UState
    {
        public override void OnEnterState()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();
<<<<<<< Updated upstream
            if (paris.Location?.IntroIncidentConfig != null)
                calendar.Schedule(paris.Location.IntroIncidentConfig.GetIncident(), calendar.Today);
=======
            calendar.Schedule(paris.Location?.IntroIncident, calendar.Today);
>>>>>>> Stashed changes
        }
    }
}
