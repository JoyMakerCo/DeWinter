using System;
using Core;

namespace Ambition
{
	public class AdjustDevotionCmd : ICommand<AdjustValueVO>
	{
		public void Execute (AdjustValueVO vo)
		{
			CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
			NotableVO notable = Array.Find(model.Notables, n=>n.Name == vo.Type);
			if (vo.IsRequest && notable != null)
			{
				notable.Devotion += (int)vo.Amount;
				vo.IsRequest = false;
				AmbitionApp.SendMessage<AdjustValueVO>(vo);
			}
		}
	}
}
