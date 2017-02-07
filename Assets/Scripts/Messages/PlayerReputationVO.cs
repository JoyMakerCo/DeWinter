using System;

namespace DeWinter
{
	public class PlayerReputationVO
	{
		public int Reputation;
		public int ReputationLevel;

		public PlayerReputationVO (int reputation, int level)
		{
			Reputation = reputation;
			ReputationLevel = level;
		}
	}
}