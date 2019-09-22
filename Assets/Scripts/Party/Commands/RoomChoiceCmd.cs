using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class RoomChoiceCmd : ICommand<RoomVO>
	{
		public void Execute (RoomVO room)
		{
			//if (!room.Cleared && (room.MoveThroughChance > 0) && !room.HostHere)
			{
				AmbitionApp.OpenDialog<RoomVO>(DialogConsts.CHOOSE_ROOM, room);
			}
		}
	}
}
