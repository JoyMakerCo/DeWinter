using Core;
using System.Collections.Generic;

namespace DeWinter
{
	public class RewardCmd : ICommand<RewardVO>
	{
		public void Execute(RewardVO reward)
		{
			AdjustValueVO vo = new AdjustValueVO();
			foreach(KeyValuePair<string, double> kvp in reward.Balances)
			{
				vo.Type = kvp.Key;
				vo.Amount = kvp.Value;
				vo.IsRequest = true;
				DeWinterApp.SendMessage<AdjustValueVO>(vo);
			}
		}
	}
}