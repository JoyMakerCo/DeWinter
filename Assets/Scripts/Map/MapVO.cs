using System;
using System.Collections;

namespace DeWinter
{
	public class MapVO
	{
		public string Name;
		public RoomVO[,] Rooms;

		public RoomVO Entrance;

		public int Width
		{
			get { return (Rooms != null) ? Rooms.GetLength(0) : 0; }
		}

		public int Depth
		{
			get { return (Rooms != null) ? Rooms.GetLength(1) : 0; }
		}

		// Default Constructor
		public MapVO () {}
		public MapVO (int width, int depth)
		{
			Rooms = new RoomVO[width, depth];
		}
	}
}