using System;
using UFlow;

namespace Ambition
{
	public class EndIncidentState : UState
	{
		public override void OnEnterState ()
		{
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            if (calendar.Incident != null) calendar.IncidentQueue.Pop();

            if (calendar.Incident != null) AmbitionApp.SendMessage(calendar.Incident);
            else AmbitionApp.SendMessage(IncidentMessages.END_INCIDENTS);
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC, 2f);
	    }
	}
}
