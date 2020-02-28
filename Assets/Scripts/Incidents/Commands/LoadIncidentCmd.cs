using System;
using System.IO;
using UnityEngine;

namespace Ambition
{
    public class LoadIncidentCmd : Core.ICommand<string>
    {
        public void Execute(string incidentID)
        {
            IncidentConfig config = UnityEngine.Resources.Load<IncidentConfig>(IncidentConsts.DIRECTORY + incidentID);
            IncidentVO incident = config?.GetIncident();
            if (incident != null)
            {
                AmbitionApp.SendMessage<IncidentVO>(CalendarMessages.SCHEDULE, incident);
            }
        }
    }
}
