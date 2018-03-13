using System;
using UFlow;

namespace Ambition
{
	public class CheckEndIncidentState : UState
	{
		public override void OnEnterState ()
		{
			IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
			if (model.Moment == null)
			{
				model.Incident = null;
				AmbitionApp.CloseDialog(IncidentView.DIALOG_ID);
			}
	    }
	}
}
