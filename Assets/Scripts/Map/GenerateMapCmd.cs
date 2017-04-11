using System;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class GenerateMapCmd : ICommand<Party>
	{
		private MapModel _model;
		private MapVO _map;
		private Random _rnd;
		private Party _party;
		private int _capacity;

		public void Execute(Party party)
		{
			_party = party;
			_model = DeWinterApp.GetModel<MapModel>();
			_rnd = new Random();

			// Determine if the party uses a preset.
			if (_party.tutorial)
			{
				_map = _model.Maps["Tutorial"];
			}

			// Build a new map from scratch.
			else
			{
				switch (party.partySize)
				{
				case 1:
					_map = new MapVO(3,3);
	                break;
	            case 2:
					_map = new MapVO(5,3);
	                break;
	            case 3:
					_map = new MapVO(9,3);
					break;
				}
				_capacity = _map.Rooms.Length;

				// Recursively build the map.
				_map.Entrance = BuildRoom(_rnd.Next(_map.Width), 0);
			}

			_map.Entrance.Difficulty = 0;
			_map.Entrance.Cleared = true;
			_map.Entrance.Name = "The Vestibule";
			_map.Entrance.Features = new string[0];
			_map.Entrance.Guests = new GuestVO[0];

			// Fill in the blanks
			foreach(RoomVO room in _map.Rooms)
			{
				if (room != null)
				{
					if (string.IsNullOrEmpty(room.Name))
						room.Name = GenerateRandomName();

					if (room.Features == null)
						room.Features = GetRandomFeatures();

					if (room.Difficulty == 0)
						room.Difficulty = 1 + _rnd.Next(5);

					if (room.Guests == null)
						room.Guests = GenerateGuests(room.Difficulty);

					if (room.Rewards == null)
						room.Rewards = GenerateRewards();

					if (room.Neighbors == null)
						FindNeighbors(room);
				}
			}

			PopulateEnemies();

			_model.Map = _map;
		}

// TODO: Floorplan map building
		private RoomVO BuildRoom(int X, int Y)
		{
			if (!ValidCoords(X,Y) || _rnd.Next(_map.Rooms.Length) > _capacity) return null;
			if (_map.Rooms[X,Y] is RoomVO) return _map.Rooms[X,Y];

			RoomVO room = new RoomVO();

			_map.Rooms[X,Y] = room;
// TODO: Update this when map drawing gets more fleshed out
room.Shape = new UnityEngine.Vector2[]{new UnityEngine.Vector2(X,Y)};

			room.Neighbors = new RoomVO[4];
			room.Neighbors[1] = BuildRoom(X+1, Y); // East
			room.Neighbors[3] = BuildRoom(X-1, Y); // West
			room.Neighbors[0] = BuildRoom(X, Y+1); // North
			room.Neighbors[2] = BuildRoom(X, Y-1); // South

			_capacity--;
			if (_capacity == 0)
			{
				room.Features = new string[] { PartyConstants.HOST };
			}
			return room;
		}

		private string GenerateRandomName()
	    {
            string adjective = GetRandomDescriptor(_model.RoomAdjectives);
			string noun = GetRandomDescriptor(_model.RoomNames);
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

		private void FindNeighbors(RoomVO room)
		{
			// This seems convoluted, but in future iterations of the map,
			// rooms may have more than one door in each direction.
			// This logic will change significatnly at that iteration. 
			RoomVO[] result = new RoomVO[4];
			for (int x=_map.Rooms.GetLength(0)-1; x>=0; x--)
			{
				for (int y=_map.Rooms.GetLength(1)-1; y>=0; y--)
				{
					if (_map.Rooms[x,y] == room)
					{
						result[0] = ValidCoords(x,y+1) ? _map.Rooms[x,y+1] : null;
						result[1] = ValidCoords(x+1,y) ? _map.Rooms[x+1,y] : null;
						result[2] = ValidCoords(x,y-1) ? _map.Rooms[x,y-1] : null;
						result[3] = ValidCoords(x-1,y) ? _map.Rooms[x-1,y] : null;
						_map.Rooms[x,y].Neighbors = result;
						return;
					}
				}
			}
		}

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
				result[i] = new GuestVO(_rnd.Next(opinionMin, opinionMax), _rnd.Next(interestMin, interestMax));
			}
			return result;
		}

		private Reward [] GenerateRewards()
    	{
    		return new Reward[] {
				new Reward(_party, "Random", 0),
				new Reward(_party, "Random", 1),
				new Reward(_party, "Random", 2),
				new Reward(_party, "Random", 3),
				new Reward(_party, "Random", 4),
				new Reward(_party, "Random", 5),
				new Reward(_party, "Random", 6)
    		};
	    }

	    private void PopulateEnemies()
	    {
			List<Enemy> enemies = EnemyInventory.enemyInventory.FindAll(e => e.Faction == GameData.tonightsParty.faction);
			int X, Y;
			foreach (Enemy e in enemies)
	        {
				X = _rnd.Next(_map.Width);
				Y = _rnd.Next(_map.Depth);
				if(_map.Rooms[X, Y] != null
					&& _map.Rooms[X, Y] != _map.Entrance
					&& _rnd.Next(0,2) == 0)
                {
					_map.Rooms[X, Y].Enemies.Add(e);
                }
	        }
	    }

	    private bool ValidCoords(int x, int y)
	    {
	    	return x >= 0
	    		&& y >= 0
	    		&& x < _map.Width
	    		&& y < _map.Depth;
	    }
	}
}