using System;
using Core;
using UFlow;

namespace Ambition
{
	public class StartIncidentState : UState
	{
		private const string MODAL_ID = "INCIDENT";

		public override void OnEnterState ()
		{
			IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
			AmbitionApp.OpenDialog(MODAL_ID);
			model.Moment = model.Incident.Moments[0];
	    }
	}
}
