using System;
using Core;

namespace DeWinter
{
	public class NewGameCmd : ICommand
	{
		public void Execute ()
		{
			EventModel emod = DeWinterApp.GetModel<EventModel>();
			emod.SelectedEvent = emod.eventInventories["intro"][0];

			// Populate initial parties
			CalendarModel cmod = DeWinterApp.GetModel<CalendarModel>();
			cmod.Parties.Add(cmod.DaysFromNow(1), new Party(1));
			cmod.Parties.Add(cmod.DaysFromNow(4), new Party(1));
			if (new Random().Next(3) == 0)
				cmod.Parties.Add(cmod.DaysFromNow(4), new Party(1));

			DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE, "Game_Estate");
		}
	}
}