using System;
using UFlow;

namespace Ambition
{
	public class EndIncidentState : UState
	{
		public override void OnEnterState ()
		{
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            calendar.EndIncident();
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC, 2f);
	    }
	}
}
