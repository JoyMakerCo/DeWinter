using System;
using Core;

namespace DeWinter
{
	public class NavigateToRoomCmd : ICommand<RoomVO>
	{
		public void Execute(RoomVO room)
		{
			MapModel model = DeWinterApp.GetModel<MapModel>();
			Random rnd = new Random();

			// Make sure the player can move to the next room
			if (rnd.Next(100) < model.MoveThroughChance)
			{

				// Make sure the room is connected
				foreach (Door door in model.Room.Doors)
				{
					if (room == door.Room)
					{
						model.Room = door.Room;
						DeWinterApp.SendMessage<RoomVO>(MapMessage.ENTERED_ROOM, room);
						return;
					}
				}
			}
			else
			{
				// Denied! Player can't move through the room yet.
			}
		}
	}
}