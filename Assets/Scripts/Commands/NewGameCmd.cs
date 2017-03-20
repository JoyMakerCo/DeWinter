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
			DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE, "Game_Estate");
		}
	}
}