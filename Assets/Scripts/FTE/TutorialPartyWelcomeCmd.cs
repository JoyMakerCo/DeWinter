using System;
using Core;

namespace Ambition
{
	public class TutorialPartyWelcomeCmd : ICommand<RoomVO>
	{
		public void Execute (RoomVO room)
		{
			AmbitionApp.OpenMessageDialog("party_tutorial_welcome_dialog");
			AmbitionApp.UnregisterCommand<TutorialPartyWelcomeCmd, RoomVO>();
			AmbitionApp.RegisterCommand<TutorialPartyCmd, RoomVO>();
		}
	}
}