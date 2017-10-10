using System;
using Core;

namespace Ambition
{
	public class TutorialPartyCmd : ICommand<RoomVO>
	{
		public void Execute (RoomVO room)
		{
			AmbitionApp.OpenDialog("WORK_THE_ROOM_TUTORIAL");
			AmbitionApp.UnregisterCommand<TutorialPartyCmd, RoomVO>();
		}
	}
}