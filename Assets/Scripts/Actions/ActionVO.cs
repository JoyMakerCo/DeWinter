using System;
using System.Collections.Generic;

namespace Actions
{
	public class ActionVO
	{
		public string Name;
		public DateTime Completed;

		// Criteria
		public Dictionary<string, double> Balances;
// TODO: Replace 'object' with ItemVO
		public Dictionary<string, object> Items;

		// Reward
		public RewardVO Reward;
	}
}