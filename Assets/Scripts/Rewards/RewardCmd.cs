using Core;
using System.Collections.Generic;

namespace DeWinter
{
	public class RewardCmd : ICommand<RewardVO>
	{
		public void Execute(RewardVO reward)
		{
			GameDataModel gameData = DeWinterApp.GetModel<GameDataModel>();
			double bal;
			foreach(KeyValuePair<string, double> kvp in reward.Balances)
			{
				if (gameData.Balances.TryGetValue(kvp.Key, out bal))
				{
					gameData.Balances.Add(kvp.Key, kvp.Value);
				}
				else
				{
					gameData.Balances[kvp.Key] += kvp.Value;
				}
			}
		}
	}
}