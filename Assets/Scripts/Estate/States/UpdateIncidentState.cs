using System.Linq;
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
                    QueueIncident(calendar, incident);
                else
                    calendar.Timeline[incident.Date].Remove(incident);
            }

            incidents = calendar.Unscheduled.OfType<IncidentVO>().ToArray();
            foreach (IncidentVO incident in incidents)
            {
                if (incident.Requirements.Length > 0 && AmbitionApp.CheckRequirements(incident.Requirements))
                {
                    calendar.Schedule(incident, calendar.Today);
                    QueueIncident(calendar, incident);
                }
            }
            if (calendar.Incident != null)
                AmbitionApp.SendMessage(IncidentMessages.START_INCIDENT);
		}

        private void QueueIncident(CalendarModel model, IncidentVO incident)
        {
            if (!model.IncidentQueue.Contains(incident))
            {
                model.IncidentQueue = new Stack<IncidentVO>(model.IncidentQueue.Append(incident));
            }
            AmbitionApp.SendMessage(incident);
        }
    }
}
