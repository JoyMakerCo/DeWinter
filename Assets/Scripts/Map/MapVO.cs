using System;

namespace DeWinter
{
	public class MapVO
	{
		public string Name;
		public RoomVO[,] Rooms;

		public RoomVO Entrance
		{
			get { return (Rooms != null) ? Rooms[Width >> 1, 0] : null; }
		}

		public int NumRooms
		{
			get { return Rooms != null ? Rooms.Length : 0; }
		}

		public int Width
		{
			get { return (Rooms != null) ? Rooms.GetLength(0) : 0; }
		}

		public int Depth
		{
			get { return (Rooms != null) ? Rooms.GetLength(1) : 0; }
		}

		public MapVO () {}
		public MapVO (int width, int depth)
		{
			Rooms = new RoomVO[width, depth];
		}
	}
}