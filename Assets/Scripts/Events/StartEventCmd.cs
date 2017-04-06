using System;
using Core;

namespace DeWinter
{
	public class StartEventCmd : ICommand<CalendarDayVO>
	{
		private const string MODAL_ID = "EventPopUpModal";

		public void Execute (CalendarDayVO day)
		{
			Random rnd = new Random();
			CalendarModel cmod = DeWinterApp.GetModel<CalendarModel>();
			EventModel emod = DeWinterApp.GetModel<EventModel>();
			if (emod.SelectedEvent == null
				&& cmod.Day >= 2
				&& (rnd.Next(100) < emod.EventChance))
			{
				EventVO [] events = emod.eventInventories["night"];

//			//Select the Event
//	        WeightedSelection();

				emod.SelectedEvent = events[rnd.Next(events.Length)];
				emod.SelectedEvent.currentStageIndex = 0;
			}
			DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE, "Game_Estate");
	    }
	}
}