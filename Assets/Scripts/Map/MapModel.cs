using System;
using Core;

namespace DeWinter
{
	public class MapModel : IModel
	{
		public MapVO Map;

		private RoomVO _room;
		public RoomVO Room
		{
			get { return _room; }
			set {
				_room = value;
				_room.Revealed = true;
			}
		}

		public MapModel (MapVO map)
		{
			Map = map;
			Room = map.Entrance;
		}

		public bool IsEntrance
		{
			get { return Room == Map.Entrance; }
		}
	}
}