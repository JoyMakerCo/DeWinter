using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace Ambition
{
	public class MapModel : DocumentModel
	{
		public MapModel() : base("MapData") {}

		private RoomVO _room;
		public RoomVO Room
		{
			get { return _room; }
			set {
				_room = value;
				AmbitionApp.SendMessage<RoomVO>(_room);
			}
		}

		public RoomVO Entrance
		{
			get { return _map != null ? _map.Entrance : null; }
		}

		protected MapVO _map;
		public MapVO Map
		{
			get { return _map; }
			set {
				_map = value;
				AmbitionApp.SendMessage<MapVO>(_map);
				if (_map != null) Room = _map.Entrance; // Also sends a message
			}
		}

		[JsonProperty("roomAdjectiveList")]
		public string [] RoomAdjectives;

		[JsonProperty("roomNounList")]
		public string [] RoomNames;

		[JsonProperty("maps")]
		public Dictionary<string, MapVO> Maps = new Dictionary<string, MapVO>();
	}
}
