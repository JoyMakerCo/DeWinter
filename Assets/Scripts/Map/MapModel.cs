using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace DeWinter
{
	public class MapModel : DocumentModel
	{
		public MapModel() : base("MapData")
		{
		// TODO: MapData presets, map to string IDs
			MakeTutorialMap();
		}

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

		[JsonProperty("maps")]
		public Dictionary<string, MapVO> Maps;

		// TODO: MapData presets, map to string IDs
		private void MakeTutorialMap()
		{
			Maps = new Dictionary<string, MapVO>();
			MapVO map = new MapVO(3,4);
			map.Name = "Tutorial";
			map.Rooms[0, 3] = new RoomVO();
	        map.Rooms[0, 3].Difficulty = 3;
			map.Rooms[0, 3].Guests = GenerateGuests(3,4);

			map.Rooms[1, 3] = new RoomVO("The Host's Quarters");
			map.Rooms[1, 3].Difficulty = 6;
	        map.Rooms[1, 3].Features = new string[]{PartyConstants.HOST};

	        map.Rooms[2, 3] = new RoomVO();
			map.Rooms[2, 3].Difficulty = 3;
	        map.Rooms[2, 3].Guests = GenerateGuests(3, 4);
	        //Row 2
	        map.Rooms[0, 2] = new RoomVO();
			map.Rooms[0, 2].Difficulty = 2;
	        map.Rooms[0, 2].Guests = GenerateGuests(2, 4);

	        map.Rooms[1, 2] = new RoomVO();
			map.Rooms[1, 2].Difficulty = 2;
	        map.Rooms[1, 2].Guests = GenerateGuests(2, 3);
	        map.Rooms[1, 2].TurnTimer = 6.25f;
			map.Rooms[1, 2].IsImpassible=true;

	        map.Rooms[2, 2] = new RoomVO();
			map.Rooms[2, 2].Difficulty = 3;
	        map.Rooms[2, 2].Guests = GenerateGuests(3, 3);
	        //Row 1
	        map.Rooms[1, 1] = new RoomVO();
			map.Rooms[1, 1].Difficulty = 1;
	        map.Rooms[1, 1].Guests = GenerateGuests(1, 2);
	        map.Rooms[1, 1].TurnTimer = 7.5f;
			map.Rooms[1, 1].IsTutorial = true;
			map.Rooms[1, 1].IsImpassible=true;
	        //Row 0
	        map.Rooms[1, 0] = map.Entrance = new RoomVO("The Vestibule");

	        Maps[map.Name] = map;
		}

		private Guest[] GenerateGuests(int difficulty, int numGuests)
		{
			int[] ranges;
			switch (difficulty)
	        {
	            case 1:
	            	ranges = new int[]{25, 51, 6, 10};
	                break;
	            case 2:
					ranges = new int[]{25, 46, 5, 9};
	                break;
	            case 3:
					ranges = new int[]{25, 41, 4, 8};
	                break;
	            case 4:
					ranges = new int[]{25, 36, 3, 7};
	                break;
	            case 5:
					ranges = new int[]{20, 31, 2, 6};
	                break;
	            default:
	            	return new Guest[0];
	        }
			Guest[] guests = new Guest[numGuests];
			Random rnd = new Random();
			for (int i=0; i<numGuests; i++)
			{
				guests[i] = new Guest(rnd.Next(ranges[0], ranges[1]), rnd.Next(ranges[2], ranges[3]));
			}
			return guests;
		}
	}
}