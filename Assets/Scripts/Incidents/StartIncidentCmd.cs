using System;
using Core;

namespace Ambition
{
    public class StartIncidentCmd : ICommand<string>
    {
        public void Execute(string incidentID)
        {
            CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
            model.Incident = model.Find(incidentID);
        }
    }
}
