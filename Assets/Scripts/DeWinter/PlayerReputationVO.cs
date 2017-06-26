using System;

namespace Ambition
{
	public class PlayerReputationVO
	{
		public int Reputation;
		public int Level;

		public PlayerReputationVO (int reputation, int level)
		{
			this.Reputation = reputation;
			this.Level = level;
		}
	}
}