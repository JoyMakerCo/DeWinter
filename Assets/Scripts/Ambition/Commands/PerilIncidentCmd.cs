using System;
using System.Linq;
using Core;
namespace Ambition
{
    public class PerilIncidentCmd : ICommand<int>
    {
        public void Execute(int peril)
        {
/*
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            if (peril >= 100 && model.PerilIncidents.Count > 0)
            {
                List<IncidentVO> incidents = new List<IncidentVO>();
                foreach(IncidentVO incident in model.Incidents.Values)
                {
                    if (incident.)
                }
                CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                IncidentVO[] incidents = model.PerilIncidents.Where(i => AmbitionApp.CheckRequirements(i.Requirements)).ToArray();
                IncidentVO incident = Util.RNG.TakeRandom(incidents);
                calendar.Schedule(incident, calendar.Today);
                model.PerilIncidents.Remove(incident);
            }
            */
        }
    }
}
