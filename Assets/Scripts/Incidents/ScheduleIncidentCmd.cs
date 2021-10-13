using Core;
using System;

namespace Ambition
{
    public class ScheduleIncidentCmd : ICommand<IncidentVO>
    {
        public void Execute(IncidentVO incident)
        {
            AmbitionApp.GetModel<IncidentModel>().Schedule(incident);
        }
    }
}
