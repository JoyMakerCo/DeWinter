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
			Party p = new Party(1);
			cmod.AddParty(1, new Party(1));
			cmod.AddParty(4, p);
			if (new Random().Next(3) == 0)
			{
				cmod.AddParty(4, new Party(p.faction));
			}

			DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE, "Game_Estate");
		}
	}
}