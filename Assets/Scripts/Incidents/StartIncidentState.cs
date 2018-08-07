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
            if (model.Incident != null) model.Moment = model.Incident.Nodes[0];
		}
	}
}
