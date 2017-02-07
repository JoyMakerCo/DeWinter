﻿using System;

namespace DeWinter
{
	public class MapVO
	{
		public string Name;
		public RoomVO Entrance;
		public Party Party;
		public RoomVO [] Rooms;

		public MapVO () {}

		public MapVO (Party p)
		{
			Party = p;
		}
	}
}