using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ambition
{
	public class MapVO
	{
		[JsonProperty("name")]
		public string Name;

		public RoomVO Entrance;

		public RoomVO[,] Rooms;

		[JsonProperty("rooms")]
		protected RoomVO[] _rooms
		{
			set
			{
				int Y=0;
				int X=0;
				Entrance = value[0];

				foreach (RoomVO room in value)
				{
					if (room.Coords[0] > X)
						X = room.Coords[0];
					if (room.Coords[1] > Y)
						Y = room.Coords[1];
				}

				Rooms = new RoomVO[X+1, Y+1];
				foreach (RoomVO room in value)
				{
					X = room.Coords[0];
					Y = room.Coords[1];
					Rooms[X, Y] = room;
				}
			}
		}
	}
}
