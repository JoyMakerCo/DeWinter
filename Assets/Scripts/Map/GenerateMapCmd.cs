using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace Ambition
{
	public class GenerateMapCmd : ICommand<PartyVO>
	{
		private Random _rnd;

		public void Execute(PartyVO party)
		{
			MapModel model;
			MapVO map;
			model = AmbitionApp.GetModel<MapModel>();
			_rnd = new Random();

			// Determine if the party uses a preset, or build a map from scratch
			if (!model.Maps.TryGetValue(party.MapID, out map))
			{
				map = BuildRandomMap(party.partySize);
			}

			// Fill in the blanks
			foreach(RoomVO room in map.Rooms)
			{
				if (room != null)
				{
					if (string.IsNullOrEmpty(room.Name))
						room.Name = GenerateRandomName(model.RoomAdjectives, model.RoomNames);

					if (room.Features == null)
						room.Features = GetRandomFeatures();

					if (room.Difficulty == 0 && room != map.Entrance)
						room.Difficulty = 1 + _rnd.Next(5);

					room.MoveThroughChance = GenerateMoveThroughChance(room);

					room.Neighbors = FindNeighbors(room, map);


					if (room != null
						&& map.Entrance != room
						&& !room.HostHere
						&& _rnd.Next(2) == 0)
					{
						
					}
				}

				// Setup the Vestibule
				map.Entrance.Difficulty = 0;
				map.Entrance.Cleared = true;
				if (map.Entrance.Name == null)
					map.Entrance.Name = "The Vestibule";
				map.Entrance.Features = new string[0];
			}
			party.Enemies = PopulateEnemies(map, EnemyInventory.enemyInventory.FindAll(e=>e.Faction == party.Faction));
			model.Map = map;
		}

		private RoomVO[] FindNeighbors(RoomVO room, MapVO map)
		{
			List<RoomVO> result = new List<RoomVO>();
			int[] xy = room.Coords;
			if (ValidCoords(map.Rooms, xy[0]-1, xy[1])) result.Add(map.Rooms[xy[0]-1, xy[1]]);
			if (ValidCoords(map.Rooms, xy[0]+1, xy[1])) result.Add(map.Rooms[xy[0]+1, xy[1]]);
			if (ValidCoords(map.Rooms, xy[0], xy[1]-1)) result.Add(map.Rooms[xy[0], xy[1]-1]);
			if (ValidCoords(map.Rooms, xy[0], xy[1]+1)) result.Add(map.Rooms[xy[0], xy[1]+1]);

			return result.FindAll(r => r != null).ToArray();
		}

		private MapVO BuildRandomMap(int partySize)
		{
			MapVO map = new MapVO();
			switch (partySize)
			{
				case 1:
					map.Rooms = new RoomVO[3,3];
					break;
	            case 2:
					map.Rooms = new RoomVO[5,3];
	                break;
	            case 3:
					map.Rooms = new RoomVO[9,3];
					break;
			}

			// TODO: Floorplan map building
			for (int i=map.Rooms.GetLength(0)-1; i>=0; i--)
			{
				for (int j=map.Rooms.GetLength(1)-1; j>=0; j--)
				{
					map.Rooms[i,j] = new RoomVO();
					map.Rooms[i,j].Coords = new int[]{i,j};
				}
			}
			return map;
		}


		private string GenerateRandomName(string[] adjectives, string [] names)
	    {
            string adjective = GetRandomDescriptor(adjectives);
			string noun = GetRandomDescriptor(names);
            return "The " + adjective + " " + noun;
	    }

	    private string GetRandomDescriptor(string[] list)
	    {
	    	int index = _rnd.Next(list.Length);
	    	return list[index];
	    }

	    private string[] GetRandomFeatures()
	    {
	    	List<string> result = new List<string>();

			//TODO: make features abstract and configurable
	    	int punchBowlChance = 33;
	    	if (_rnd.Next(100) < punchBowlChance)
	    		result.Add(PartyConstants.PUNCHBOWL);

	    	return result.ToArray();
	    }

		private int GenerateMoveThroughChance(RoomVO room)
		{
			if (room.MoveThroughChance >= 0 && !room.Cleared) return room.MoveThroughChance;
			return 90 - (room.Cleared ? 0 : room.Difficulty * 10);
		}

		// TODO: Generate random guests on the fly upon entering the appropriate room
		// Guest List should be prepopulated only with Enemies and Notables,
		// and arrays already at appropriate length with null entries
		private GuestVO[] GenerateGuests(int difficulty)
	    {
	        switch (difficulty)
	        {
	            case 1:
					return generateGuestList(4, 25, 51, 6, 10);
				case 2:
					return generateGuestList(4, 25, 46, 5, 9);
				case 3:
					return generateGuestList(4, 25, 41, 4, 8);
	            case 4:
					return generateGuestList(4, 25, 36, 3, 7);
	            case 5:
					return generateGuestList(4, 20, 31, 2, 6);
	        }
	        return new GuestVO[0];
		}

		private GuestVO[] generateGuestList(int count, int opinionMin, int opinionMax, int interestMin, int interestMax)
		{
			GuestVO[] result = new GuestVO[count];
			for (int i=0; i<count; i++)
			{
				result[i] = new GuestVO();
				result[i].Opinion = _rnd.Next(opinionMin, opinionMax);
			}
			return result;
		}

	    private EnemyVO[] PopulateEnemies(MapVO map, List<EnemyVO> enemies)
	    {
	    	int W = map.Rooms.GetLength(0);
			int H = map.Rooms.GetLength(1);
			RoomVO room;
			List<EnemyVO> result=new List<EnemyVO>();
			foreach (EnemyVO enemy in enemies)
			{
				do
				{
					room = map.Rooms[_rnd.Next(W), _rnd.Next(H)];
				}
				while (room == null);
				if (room != map.Entrance
					&& !room.HostHere
					&& _rnd.Next(2) == 0)
				{
					room.Enemies.Add(enemy);
					result.Add(enemy);
				}
			}
			return result.ToArray();
	    }

	    private bool ValidCoords(RoomVO[,] grid, int x, int y)
	    {
	    	return x >= 0
	    		&& y >= 0
	    		&& x < grid.GetLength(0)
	    		&& y < grid.GetLength(1);
	    }
	}
}