using System;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
	public class UpdateIncidentState : UState
	{
		public override void OnEnterState ()
		{
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            IncidentVO[] incidents = calendar.GetEvents<IncidentVO>();
            foreach(IncidentVO incident in incidents)
            {
                if (AmbitionApp.CheckRequirements(incident.Requirements))
                    calendar.QueueIncident(incident);
                else
                    calendar.Timeline[incident.Date].Remove(incident);
            }

            foreach(IncidentVO incident in calendar.Unscheduled)
            {
                if (incident.Requirements.Length > 0 && AmbitionApp.CheckRequirements(incident.Requirements))
                {
                    calendar.Schedule(incident, calendar.Today);
                    calendar.QueueIncident(incident);
                }
            }
            if (calendar.Incident != null)
                AmbitionApp.SendMessage(calendar.Incident);
		}
	}
}
