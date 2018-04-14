using System;
using Core;
using UFlow;

namespace Ambition
{
	public class StartIncidentState : UState
	{
		override public void OnEnterState()
		{
			IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
			if (model.Incident != null) model.Moment = model.Incident.Moments[0];
		}
	}
}
