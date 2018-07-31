using System;
using Core;

namespace Ambition
{
    public class StartIncidentCmd : ICommand<string>
    {
        public void Execute(string incidentID)
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            model.Incident = model.Find(incidentID);
        }
    }
}
