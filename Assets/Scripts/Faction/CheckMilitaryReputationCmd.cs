using System;
using Core;

namespace Ambition
{
	public class CheckMilitaryReputationCmd : ICommand<AdjustValueVO>
	{
		public void Execute (AdjustValueVO vo)
		{
			if (!vo.IsRequest && vo.Type == "Military")
			{
				FactionModel fmod = DeWinterApp.GetModel<FactionModel>();
				if (fmod["Military"].ReputationLevel >= 9)
		        {
					EnemyInventory.enemyInventory.RemoveAll(e => e.Faction == "Military");
		        }
		    }
		}
	}
}