using System;
using Core;

namespace DeWinter
{
	public class EventPopupCmd : ICommand<SceneFadeInOut>
	{
// TODO: Refactor to work with dialog system
		public void Execute (SceneFadeInOut screenFader)
		{
			int day = DeWinterApp.GetModel<CalendarModel>().Day;
			int chance = DeWinterApp.GetModel<EventModel>().EventChance;
			if (day == 0)
	        {
	            screenFader.gameObject.SendMessage("CreateEventPopUp", "intro");
	        }
	        //All the Other Events
	        else if (day > 2 && ((new Random()).Next(0, 100) < chance))
	        {
	            screenFader.gameObject.SendMessage("CreateEventPopUp", "night");
	        }
	    }
	}
}