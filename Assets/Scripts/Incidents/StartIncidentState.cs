using System;
using Core;
using UFlow;

namespace Ambition
{
	public class StartIncidentState : UState
	{
		override public void OnEnterState()
		{
			CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
            if (model.Incident != null)
            {
                AmbitionApp.SendMessage(IncidentMessages.START_INCIDENT, model.Incident);
                model.Moment = model.Incident.Nodes[0];
                AmbitionApp.SendMessage(model.Incident.GetLinkData(0));
            }
		}
	}
}
