using System;
using Core;

namespace DeWinter
{
	public class TutorialPartyWelcomeCmd : ICommand<RoomVO>
	{
		public void Execute (DateTime date)
		{
			DeWinterApp.OpenMessageDialog("party_tutorial_welcome_dialog");
			DeWinterApp.UnregisterCommand<TutorialPartyWelcomeCmd, RoomVO>();
			DeWinterApp.RegisterCommand<TutorialPartyCmd, RoomVO>();
		}
	}
}