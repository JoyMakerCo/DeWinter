using System;
using Core;

namespace Ambition
{
	public class MakeNotableEnemyCmd : ICommand<NotableVO>
	{
		public void Execute (NotableVO notable)
		{
			EnemyVO enemy = new EnemyVO(notable as GuestVO);
			enemy.Faction = notable.Faction;
		}
	}
}