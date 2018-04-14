using System;
using UFlow;

namespace Ambition
{
	public class StartEstateState : UState
	{
		public override void OnEnterState ()
		{
			CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
			AmbitionApp.SendMessage<DateTime>(calendar.Today);
		}
	}
}
