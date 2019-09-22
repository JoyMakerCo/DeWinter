using Core;
using System.Collections.Generic;
using System.Linq;

namespace Ambition
{
    public class UpdateIncidentsCmd : ICommand
    {
        public void Execute()
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            IEnumerable<IncidentVO> unscheduled = calendar.FindUnscheduled<IncidentVO>(i =>
                (i.Requirements?.Length ?? 0) > 0
                && AmbitionApp.CheckRequirements((i as IncidentVO).Requirements));
            model.IncidentQueue = calendar.GetEvents<IncidentVO>().ToList();
            model.IncidentQueue.AddRange(unscheduled);
        }
    }
}
