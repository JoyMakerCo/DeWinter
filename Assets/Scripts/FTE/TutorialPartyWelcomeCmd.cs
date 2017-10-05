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

			// Not eloquent, but FTEs rarely are.
			UnityEngine.GameObject.Find("LeaveThePartyButton").SetActive(false);
		}
	}
}