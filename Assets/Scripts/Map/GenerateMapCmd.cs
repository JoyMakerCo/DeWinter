using System;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class GenerateMapCmd : ICommand<Party>
	{
		private RoomVO [] roomList;

		public void Execute(Party party)
		{
			Random rnd = new Random();
			MapVO map = new MapVO(party);


			switch (party.partySize)
			{
			case 1:
				roomList = new RoomVO[7];
                break;
            case 2:
				roomList = new RoomVO[rnd.Next(11, 13)];
                break;
            case 3:
				roomList = new RoomVO[rnd.Next(18, 20)];
				break;
			}

			map.Entrance = roomList[0] = BuildEntrance(party.partySize, rnd);
			map.Entrance.Difficulty = 1;

			PopulateEnemies(map, rnd, roomList.Length);
			DeWinterApp.SendMessage<MapVO>(MapMessage.MAP_READY, map);
		}

		private RoomVO BuildEntrance(int size, Random rnd)
		{
			RoomVO entrance = new RoomVO("Entrance");
			return entrance;
		}

		private string GenerateRandomName(Random rnd)
	    {
//	    	GameDataModel model =  DeWinterApp.GetModel<GameDataModel>();
            string adjective = GetRandomDescriptor(GameData.roomAdjectiveList, rnd);
			string noun = GetRandomDescriptor(GameData.roomNounList, rnd);
            return "The " + adjective + " " + noun;
	    }

	    private string GetRandomDescriptor(List<string> list, Random seed)
	    {
	    	int index = seed.Next(0, list.Count);
	    	return list[index];
	    }

	    private void AddRandomFeatures(RoomVO room, Random rnd)
	    {
	    	//TODO: make features abstract and configurable
	    	int punchBowlChance = 33;
	    	if (rnd.Next(100) < punchBowlChance)
	    	{
	    		room.Features = new string[]{ PartyConstants.PUNCHBOWL };
	    	}
	    }

		private List<Guest> GenerateGuests(int difficulty, Random rnd)
	    {
			List<Guest> result = new List<Guest>();
	        switch (difficulty)
	        {
	            case 1:
					result.Add(new Guest(rnd.Next(25, 51), rnd.Next(6, 10)));
					result.Add(new Guest(rnd.Next(25, 51), rnd.Next(6, 10)));
					result.Add(new Guest(rnd.Next(25, 51), rnd.Next(6, 10)));
					result.Add(new Guest(rnd.Next(25, 51), rnd.Next(6, 10)));
	                break;
	            case 2:
					result.Add(new Guest(rnd.Next(25, 46), rnd.Next(5, 9)));
					result.Add(new Guest(rnd.Next(25, 46), rnd.Next(5, 9)));
					result.Add(new Guest(rnd.Next(25, 46), rnd.Next(5, 9)));
					result.Add(new Guest(rnd.Next(25, 46), rnd.Next(5, 9)));
	                break;
	            case 3:
					result.Add(new Guest(rnd.Next(25, 41), rnd.Next(4, 8)));
					result.Add(new Guest(rnd.Next(25, 41), rnd.Next(4, 8)));
					result.Add(new Guest(rnd.Next(25, 41), rnd.Next(4, 8)));
					result.Add(new Guest(rnd.Next(25, 41), rnd.Next(4, 8)));
	                break;
	            case 4:
					result.Add(new Guest(rnd.Next(25, 36), rnd.Next(3, 7)));
					result.Add(new Guest(rnd.Next(25, 36), rnd.Next(3, 7)));
					result.Add(new Guest(rnd.Next(25, 36), rnd.Next(3, 7)));
					result.Add(new Guest(rnd.Next(25, 36), rnd.Next(3, 7)));
	                break;
	            case 5:
					result.Add(new Guest(rnd.Next(20, 31), rnd.Next(2, 6)));
					result.Add(new Guest(rnd.Next(20, 31), rnd.Next(2, 6)));
					result.Add(new Guest(rnd.Next(20, 31), rnd.Next(2, 6)));
					result.Add(new Guest(rnd.Next(20, 31), rnd.Next(2, 6)));
	                break;
	        }
	        return result;
		}

		private List<Reward> GenerateRewards(Party party)
    	{
    		return new List<Reward> {
				new Reward(party, "Random", 0),
				new Reward(party, "Random", 1),
				new Reward(party, "Random", 2),
				new Reward(party, "Random", 3),
				new Reward(party, "Random", 4),
				new Reward(party, "Random", 5),
				new Reward(party, "Random", 6),
				new Reward(party, "Random", 7)
    		};
	    }

	    private void PopulateEnemies(MapVO map, Random rnd, int numRooms)
	    {
	    	int roomID = numRooms-1;
			Faction faction = GameData.factionList[GameData.tonightsParty.faction];
			foreach (Enemy e in EnemyInventory.enemyInventory)
	        {
				if(e.faction == faction && rnd.Next(0,2) == 0)
                {
                	roomList[roomID].Enemies.Add(e);
                }
                if (--roomID == 0)
                {
                	return;
				}
	        }
	    }
	}
}