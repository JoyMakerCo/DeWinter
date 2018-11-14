using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using UFlow;
using UnityEngine;

namespace Ambition
{
	public class GenerateMapState : UState
	{
		// TODO: Make this configurable
		private const int ENEMY_CHANCE = 50;
		private const float PHI = 1.6f;
		private const float PHI1 = .625f;
		private const int MAX_ROOM_WIDTH = 16;
		private const int MIN_ROOM_WIDTH = 4;

		private Dictionary<RoomVO, List<RoomVO>> _rooms=new Dictionary<RoomVO, List<RoomVO>>();
		private Dictionary<int[], RoomVO[]> _walls = new Dictionary<int[], RoomVO[]>();
		private MapModel _model;

        public override void OnEnterState()
        {
			MapVO map;
			RoomVO room;
            PartyVO party = AmbitionApp.GetModel<PartyModel>().Party;
            _model = AmbitionApp.GetModel<MapModel>();

			// Determine if the party uses a preset, or build a map from scratch
			if (party.MapID == null || !_model.Maps.TryGetValue(party.MapID, out map))
			{
				map = BuildRandomMap(party);
			}

			// Fill in the blanks
			for(int i=map.Rooms.Length-1; i>0; i--)
			{
				room = map.Rooms[i];
				if (room != null)
				{
					if (string.IsNullOrEmpty(room.Name))
						room.Name = GenerateRandomName(_model.RoomAdjectives, _model.RoomNames);

					if (room.Features == null)
						room.Features = GetRandomFeatures();

					if (room.Difficulty == 0 && room != map.Entrance)
						room.Difficulty = 1 + Util.RNG.Generate(0,5);

					room.MoveThroughChance = GenerateMoveThroughChance(room);
				}
			}
			PopulateEnemies(map, EnemyInventory.enemyInventory.FindAll(e=>e.Faction == party.Faction));
            map.Entrance.Cleared = true;
            _model.Map = map;
            AmbitionApp.SendMessage(MapMessage.GO_TO_ROOM, map.Entrance);
		}

		private MapVO BuildRandomMap(PartyVO party)
		{
			MapVO map = new MapVO();
			FactionVO faction = AmbitionApp.GetModel<FactionModel>().Factions[party.Faction];

			// Room size is proportional to how "baroque" the structure is.
			// Curvilinear features are a function of "modernness," which will be determined by the host.
			// Overall size of the house will be proportional to the "Importance" of the party.
			int hyphen = (int)party.Importance + Util.RNG.Generate(3); // the width of the hyphen in rooms
			int pavilion = (int)party.Importance + Util.RNG.Generate(3); // length of the pavilion in rooms
			int jut=Util.RNG.Generate(0,2);
			int spacing = (int)(Util.RNG.Generate(faction.Baroque[0], faction.Baroque[1])*.01f*MAX_ROOM_WIDTH); // Median room spacing
			float curve1 = .1f*Util.RNG.Generate(6,11);
			float delta;
			RoomVO room;

			if (spacing < MIN_ROOM_WIDTH) spacing = MIN_ROOM_WIDTH;
			int salonX = Util.RNG.Generate(spacing, spacing+spacing);
			int salonY = Util.RNG.Generate(spacing, (int)(spacing*PHI));
			int salonH = Util.RNG.Generate(salonY, spacing+spacing);
			delta = 0f;//(float)((salonX - Util.RNG.Generate(spacing, salonX)>>1));

			// Make the vestibule
			room=MakeRectRoom((int)(delta), 0, salonX - (int)(delta + delta), salonY);
			room.Cleared = true;
			room.Features = new string[0];
			room.Difficulty = 0;
			room.Name = "Vestibule";

			// Make the Salon
			room = MakeRectRoom(0, salonY, salonX, salonH);
			room.Name = "Salon";

			MakeRectRoom(salonX, salonY, spacing, salonH);
			MakeRectRoom(-spacing, salonY, spacing, salonH);

			MakeRectRoom(salonX-(int)(delta), 0, spacing+(int)(delta), salonY);
			MakeRectRoom(-spacing, 0, spacing+(int)(delta), salonY);


			for(int column=0; column<=hyphen; column++)
			{
				delta = column == hyphen ? 1 : (float)(hyphen-column);
				for (int numrooms = (int)Math.Ceiling(pavilion*curve1/delta)-1; numrooms>=0; numrooms--)
				{
					MakeRectRoom(salonX+spacing+column*spacing, -numrooms*spacing,
						spacing, numrooms > 0 ? spacing : salonY);
					MakeRectRoom(-(column+2)*spacing, -numrooms*spacing,
						spacing, numrooms > 0 ? spacing : salonY);
				}
				for (int numrooms = 1+(int)Math.Floor((float)(jut)/delta); numrooms>0; numrooms--)
				{
					MakeRectRoom(
						salonX+spacing+column*spacing,
						salonY+(numrooms-1)*salonH,
						spacing, numrooms > 1 ? spacing : salonH);
					MakeRectRoom(
						-(column+2)*spacing,
						salonY+(numrooms-1)*salonH,
						spacing, numrooms > 1 ? spacing : salonH);
				}
			}

			foreach (KeyValuePair<RoomVO, List<RoomVO>> kvp in _rooms)
			{
				kvp.Key.Doors = kvp.Value.Where(r=>r!=kvp.Key).ToArray();
			}
			map.Rooms = _rooms.Keys.ToArray();
			return map;
		}

		private RoomVO MakeRectRoom(int X, int Y, int W, int H)
		{
			return MakeRoom(X, Y, X+W, Y, X+W, Y+H, X, Y+H);
		}

		private RoomVO MakeRoom(params int[] coords)
		{
			int len = coords.Length;
			RoomVO Room;
			RoomVO room = new RoomVO();
			int[] wallCoords;
			int[] wall;
			_rooms[room] = new List<RoomVO>();
			room.Vertices = coords;
			for (int i=0; i<len; i+=2)
			{
				wallCoords = new int[4];
				for (int j=0; j<4; j++)
				{
					wallCoords[j]=coords[(i+j)%len];
				}
				wall = _walls.Keys.FirstOrDefault(w=>CoordsEqual(w, wallCoords));
				if (wall==null) _walls.Add(wallCoords, new RoomVO[]{room,null});
				else if (_walls[wall][0] != room)
				{
					_walls[wall][1] = room;
					Room = _walls[wall][0];
					_rooms[Room].Add(room);
					_rooms[room].Add(Room);
				}
			}
			return room;
		}

		public bool CoordsEqual(int[] coords1, int[] coords2)
		{
			return coords1.SequenceEqual(coords2) ||
				coords1.SequenceEqual(new int[]{coords2[2], coords2[3], coords2[0], coords2[1]});
		}

		private string GenerateRandomName(string[] adjectives, string [] names)
	    {
            string adjective = GetRandomDescriptor(adjectives);
			string noun = GetRandomDescriptor(names);
            return "The " + adjective + " " + noun;
	    }

	    private string GetRandomDescriptor(string[] list)
	    {
	    	int index = Util.RNG.Generate(0,list.Length);
	    	return list[index];
	    }

	    private string[] GetRandomFeatures()
	    {
	    	List<string> result = new List<string>();

			//TODO: make features abstract and configurable
	    	if (Util.RNG.Generate(0,100) < _model.PunchbowlChance)
	    		result.Add(PartyConstants.PUNCHBOWL);

	    	return result.ToArray();
	    }

		private int GenerateMoveThroughChance(RoomVO room)
		{
			if (room.MoveThroughChance >= 0 && !room.Cleared) return room.MoveThroughChance;
			return 90 - (room.Cleared ? 0 : room.Difficulty * 10);
		}

	    private EnemyVO[] PopulateEnemies(MapVO map, List<EnemyVO> enemies)
	    {
	    	// TODO: Isolate enemies appropriate to the current party
			List<EnemyVO> result=new List<EnemyVO>();
			if (result == null) return new EnemyVO[]{}; // No Enemies

			int numRooms = map.Rooms.Length-1;
			RoomVO room;
	    	int i;

			foreach (EnemyVO enemy in enemies)
			{
				i = Util.RNG.Generate(1,numRooms);
				room = map.Rooms[i];
				if (!room.HostHere && Util.RNG.Generate(0,100) < ENEMY_CHANCE)
				{
					if (room.Enemies == null) room.Enemies = new List<EnemyVO>();
					room.Enemies.Add(enemy);
				}
			}
			return result.ToArray();
	    }
	}
}
