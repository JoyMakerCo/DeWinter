using Core;
using System;

namespace Ambition
{
    public class ScheduleIncidentCmd : ICommand<IncidentVO>
    {
        public void Execute(IncidentVO incident)
        {
            if (incident == null) return;
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            if (incident.Date == calendar.Today && !model.IncidentQueue.Contains(incident))
            {
                model.IncidentQueue.Add(incident);
            }
            if (incident == model.Incident)
            {
                AmbitionApp.SendMessage(incident);
            }
        }
    }
}
