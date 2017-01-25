using System;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class GenerateMapCmd : ICommand<Party>
	{
		public void Execute<Party>(Party p)
		{
			Random rnd = new Random();
			MapVO map = new MapVO(p);
			map.Entrance = BuildEntrance(p.partySize, rnd);
		}

		private RoomVO BuildEntrance(int size, Random rnd)
		{
			
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

		private List<Guest> GenerateGuests(int difficulty)
	    {
			Random rnd = new Random();
			List<Guest> result;
	        switch (difficulty)
	        {
	            case 1:
					result.Add(new Guest(Random.Next(25, 51), Random.Next(6, 10)));
					result.Add(new Guest(Random.Next(25, 51), Random.Next(6, 10)));
					result.Add(new Guest(Random.Next(25, 51), Random.Next(6, 10)));
					result.Add(new Guest(Random.Next(25, 51), Random.Next(6, 10)));
	                break;
	            case 2:
					result.Add(new Guest(Random.Next(25, 46), Random.Next(5, 9)));
					result.Add(new Guest(Random.Next(25, 46), Random.Next(5, 9)));
					result.Add(new Guest(Random.Next(25, 46), Random.Next(5, 9)));
					result.Add(new Guest(Random.Next(25, 46), Random.Next(5, 9)));
	                break;
	            case 3:
					result.Add(new Guest(Random.Next(25, 41), Random.Next(4, 8)));
					result.Add(new Guest(Random.Next(25, 41), Random.Next(4, 8)));
					result.Add(new Guest(Random.Next(25, 41), Random.Next(4, 8)));
					result.Add(new Guest(Random.Next(25, 41), Random.Next(4, 8)));
	                break;
	            case 4:
					result.Add(new Guest(Random.Next(25, 36), Random.Next(3, 7)));
					result.Add(new Guest(Random.Next(25, 36), Random.Next(3, 7)));
					result.Add(new Guest(Random.Next(25, 36), Random.Next(3, 7)));
					result.Add(new Guest(Random.Next(25, 36), Random.Next(3, 7)));
	                break;
	            case 5:
					result.Add(new Guest(Random.Next(20, 31), Random.Next(2, 6)));
					result.Add(new Guest(Random.Next(20, 31), Random.Next(2, 6)));
					result.Add(new Guest(Random.Next(20, 31), Random.Next(2, 6)));
					result.Add(new Guest(Random.Next(20, 31), Random.Next(2, 6)));
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
	}
}