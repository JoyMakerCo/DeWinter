using System;
using Core;

namespace DeWinter
{
	public class AdjustDevotionCmd : ICommand<AdjustBalanceVO>
	{
		public void Execute (AdjustBalanceVO vo)
		{
			NotableVO notable;
			DevotionModel model = DeWinterApp.GetModel<DevotionModel>();
			if (model.Notables.TryGetValue(vo.Type, out notable))
			{
				notable.Devotion += (int)vo.Amount;
				model.Notables[vo.Type] = notable;
			}
		}
	}
}

