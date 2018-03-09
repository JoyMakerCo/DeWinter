using System;
using UFlow;

namespace Ambition
{
	public class StartEstateState : UState
	{
		public override void OnEnterState ()
		{
			Random rnd = new Random();
			CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
			IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
			AmbitionApp.SendMessage<DateTime>(calendar.Today);
		}
	}
}
