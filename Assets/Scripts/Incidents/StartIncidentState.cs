using System;
using Core;
using UFlow;

namespace Ambition
{
	public class StartIncidentState : UState
	{
        public override void OnEnterState()
        {
            // This will throw an exception if there's no incident.
            // THis is handled by the machine.
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            AmbitionApp.SendMessage(IncidentMessages.START_INCIDENT, model.Incident);
            model.Moment = model.Incident.Nodes[0];
            AmbitionApp.SendMessage(model.Incident.GetLinks(0));
            AmbitionApp.SendMessage(GameMessages.SHOW_HEADER, model.Incident.Name);
		}
	}
}
