using System;
using Core;

namespace DeWinter
{
	public class NavigateToRoomCmd : ICommand<RoomVO>
	{
		public void Execute<RoomVO>(RoomVO room)
		{
			MapModel model = DeWinterApp.GetModel<MapModel>();

			// Make sure the player can move to the next room
			if ((new Random()).Next(100) < MoveThrough(model.Room))
			{

				// Make sure the room is connected
				foreach (Door door in model.Room)
				{
					if (model.Room == door.Room)
					{
						model.Room = door.Room;
						DeWinterApp.SendMessage<RoomVO>(MapMessage.ENTERED_ROOM, model.Room);
						return;
					}
				}
			}
			else
			{
				// Denied! Player can't move through the room yet.
			}
		}

		private bool MoveThrough(RoomVO room)
		{
			Random rnd = new Random();
			int chance = 90 - (room.Cleared ? 0 : room.Difficulty * 10);
			return rnd.Next(100) < chance;
		}
	}
}