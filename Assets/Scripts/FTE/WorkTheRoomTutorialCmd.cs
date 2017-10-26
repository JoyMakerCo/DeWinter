using System;
using Core;

namespace Ambition
{
	public class WorkTheRoomTutorialCmd : ICommand<RoomVO>
	{
		public void Execute (RoomVO room)
		{
			AmbitionApp.OpenMessageDialog("party_tutorial_dialog");
			AmbitionApp.UnregisterCommand<WorkTheRoomTutorialCmd, RoomVO>(MapMessage.GO_TO_ROOM);
		}
	}
}
