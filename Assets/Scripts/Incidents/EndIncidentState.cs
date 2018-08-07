using System;
using UFlow;

namespace Ambition
{
	public class EndIncidentState : UState
	{
		public override void OnEnterState ()
		{
            AmbitionApp.GetModel<CalendarModel>().EndIncident();
			AmbitionApp.SendMessage<float>(AudioMessages.STOP_MUSIC, 2f);
	    }
	}
}
