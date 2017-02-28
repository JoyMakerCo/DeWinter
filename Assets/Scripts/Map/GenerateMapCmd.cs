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

			switch (party.partySize)
			{
			case 1:
				_map = new MapVO(3,4);
				_capacity = 12;
                break;
            case 2:
				_map = new MapVO(5,3);
				_capacity = 15;
                break;
            case 3:
				_map = new MapVO(7,3);
				_capacity = 21;
				break;
			}

			BuildRoom(_map.Width >> 1, 0);
			_map.Entrance.Difficulty = 1;
			PopulateEnemies();
			_model.Map = _map;
		}

// TODO: Floorplan map building
		private void BuildRoom(int X, int Y)
		{
			if (X >= 0 && X < _map.Width && Y < _map.Depth && !(_map.Rooms[X,Y] is RoomVO))
			{
				if (_rnd.Next(_map.NumRooms) < _capacity)
				{
					RoomVO room = new RoomVO(_map.NumRooms > _capacity ? GenerateRandomName() : "The Entrance");
					room.Features = GetRandomFeatures();
					room.Difficulty = 1 + _rnd.Next(5);
					room.Guests = GenerateGuests(room.Difficulty);
					room.Rewards = GenerateRewards();

					_map.Rooms[X,Y] = room;
	// TODO: Update this when map drawing gets more fleshed out
	room.Shape = new UnityEngine.Vector2[]{new UnityEngine.Vector2(X,Y)};
					_capacity--;

					BuildRoom(X, Y+1);
					BuildRoom(X-1, Y);
					BuildRoom(X+1, Y);
				}
			}
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

		private Guest[] GenerateGuests(int difficulty)
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
	        return new Guest[0];
		}

		private Guest[] generateGuestList(int count, int opinionMin, int opinionMax, int interestMin, int interestMax)
		{
			Guest[] result = new Guest[count];
			for (int i=0; i<count; i++)
			{
				result[i] = new Guest(_rnd.Next(opinionMin, opinionMax), _rnd.Next(interestMin, interestMax));
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
				new Reward(_party, "Random", 6),
				new Reward(_party, "Random", 7)
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
	}
}