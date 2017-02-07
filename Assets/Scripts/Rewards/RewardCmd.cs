using Core;
using System.Collections.Generic;

namespace DeWinter
{
	public class RewardCmd : ICommand<RewardVO>
	{
		public void Execute(RewardVO reward)
		{
			AdjustBalanceVO vo = new AdjustBalanceVO();
			foreach(KeyValuePair<string, double> kvp in reward.Balances)
			{
				vo.Type = kvp.Key;
				vo.Amount = kvp.Value;
				DeWinterApp.SendMessage<AdjustBalanceVO>(vo);
			}
		}
	}
}