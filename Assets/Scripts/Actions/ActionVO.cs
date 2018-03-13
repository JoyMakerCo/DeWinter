using System;
using System.Collections.Generic;

namespace Ambition
{
	public class ActionVO
	{
		public string Name;
		public string Type = ActionConsts.DEFAULT;

		public DateTime Completed;

		// Criteria
		public Dictionary<string, double> Balances;

// TODO: Replace 'object' with ItemVO
		public Dictionary<string, object> Items;

		// Reward
		public CommodityVO Reward;

		// New Actions as a result of completing this action
		public ActionVO[] Actions;
	}
}