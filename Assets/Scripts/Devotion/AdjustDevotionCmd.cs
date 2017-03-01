using System;
using Core;

namespace DeWinter
{
	public class AdjustDevotionCmd : ICommand<AdjustValueVO>
	{
		public void Execute (AdjustValueVO vo)
		{
			NotableVO notable;
			DevotionModel model = DeWinterApp.GetModel<DevotionModel>();
			if (vo.IsRequest && model.Notables.TryGetValue(vo.Type, out notable))
			{
				notable.Devotion += (int)vo.Amount;
				model.Notables[vo.Type] = notable;
				vo.IsRequest = false;
				DeWinterApp.SendMessage<AdjustValueVO>(vo);
			}
		}
	}
}

