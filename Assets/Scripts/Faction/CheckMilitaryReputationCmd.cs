using System;
using Core;

namespace DeWinter
{
	public class CheckMilitaryReputationCmd : ICommand
	{
		public void Execute ()
		{
			FactionModel fmod = DeWinterApp.GetModel<FactionModel>();
			if (fmod["Military"].ReputationLevel >= 9)
	        {
				EnemyInventory.enemyInventory.RemoveAll(e => e.Faction == "Military");
	        }
		}
	}
}

