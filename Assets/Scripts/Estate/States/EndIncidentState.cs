using System;
using UFlow;

namespace Ambition
{
	public class EndIncidentState : UState
	{
		public override void OnEnterState ()
		{
			IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
			IncidentVO e = model.Config;
			if (e != null) AmbitionApp.SendMessage<IncidentVO>(IncidentMessages.END_INCIDENT, e);
			model.Config = null;
			model.Moment = null;
			AmbitionApp.CloseDialog(IncidentView.DIALOG_ID);
		}
	}
}
