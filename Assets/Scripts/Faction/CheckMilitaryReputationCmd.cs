using System;
using Core;

namespace Ambition
{
	public class CheckMilitaryReputationCmd : ICommand<FactionVO>
	{
		public void Execute (FactionVO faction)
		{
			if (faction.Name == "Military" && faction.Level > 0)
			{
				EnemyInventory.enemyInventory.RemoveAll(e => e.Faction == "Military");
		    }
		}
	}
}