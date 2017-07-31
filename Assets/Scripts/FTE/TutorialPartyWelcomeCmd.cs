using System;
using Core;

namespace Ambition
{
	public class TutorialPartyWelcomeCmd : ICommand<RoomVO>
	{
		public void Execute (RoomVO room)
		{
			AmbitionApp.UnregisterCommand<TutorialPartyWelcomeCmd, RoomVO>();
			AmbitionApp.RegisterCommand<TutorialPartyCmd, RoomVO>();
		}
	}
}