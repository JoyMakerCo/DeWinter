using System;
using UFlow;

namespace Ambition
{
	public class EndIncidentState : UState
	{
		public override void OnEnterState ()
		{
			AmbitionApp.GetModel<IncidentModel>().Incident = null;
			AmbitionApp.SendMessage<float>(AudioMessages.STOP_MUSIC, 2f);
AmbitionApp.InvokeMachine("EstateController"); // TODO: Establish submachines
	    }
	}
}
