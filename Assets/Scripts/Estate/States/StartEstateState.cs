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
/*			if (model.Event == null
				&& calendar.Today >= calendar.StartDate.AddDays(2)
				&& (rnd.Next(100) < model.EventChance))
			{
				EventVO [] events = model.eventInventories[EventSetting.Night];
				model.Event = events[rnd.Next(events.Length)];
			}
			*/
		}
	}
}
