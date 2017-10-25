using System;
using Core;

namespace Ambition
{
	public class NewGameCmd : ICommand
	{
		public void Execute ()
		{
			EventModel emod = AmbitionApp.GetModel<EventModel>();
			emod.Event = emod.eventInventories["intro"][0];

			AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, "Game_Estate");
		}
	}
}