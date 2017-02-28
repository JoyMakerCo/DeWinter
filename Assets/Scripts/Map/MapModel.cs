using System;
using System.Collections;
using Core;
using Newtonsoft.Json;

namespace DeWinter
{
	public class MapModel : DocumentModel
	{
		private MapVO _map;
		public MapVO Map
		{
			get { return _map; }
			set
			{
				_map = value;
				DeWinterApp.SendMessage<MapVO>(_map);
				Room = (_map != null) ? _map.Entrance : null;
			}
		}

		private RoomVO _room;
		public RoomVO Room
		{
			get { return _room; }
			set
			{
				_room = value;
				_room.Revealed = true;
				DeWinterApp.SendMessage<RoomVO>(_room);
			}
		}

		[JsonProperty("roomAdjectiveList")]
		public string [] RoomAdjectives;

		[JsonProperty("roomNounList")]
		public string [] RoomNames;

		public MapModel() : base("MapData") {}
	}
}