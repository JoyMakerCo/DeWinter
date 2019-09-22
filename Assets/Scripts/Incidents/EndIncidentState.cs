using System;
using UFlow;

namespace Ambition
{
	public class EndIncidentState : UState
	{
        public override void OnEnterState(string[] args)
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            calendar.Complete(model.Incident);
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC, 2f);
        }
    }
}
