using System;
using Core;

namespace Ambition
{
	public class StartEventCmd : ICommand<DateTime>
	{
		private const string MODAL_ID = "EventPopUpModal";

		public void Execute (DateTime day)
		{
			Random rnd = new Random();
			CalendarModel cmod = AmbitionApp.GetModel<CalendarModel>();
			EventModel emod = AmbitionApp.GetModel<EventModel>();
			if (emod.SelectedEvent == null
				&& cmod.Today >= cmod.StartDate.AddDays(2)
				&& (rnd.Next(100) < emod.EventChance))
			{
				EventVO [] events = emod.eventInventories["night"];

//			//Select the Event
//	        WeightedSelection();

				emod.SelectedEvent = events[rnd.Next(events.Length)];
				emod.SelectedEvent.currentStageIndex = 0;
			}
//			AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, "Game_Estate");
	    }
	}
}