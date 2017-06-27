using System;
using Core;

namespace Ambition
{
	public class BuyRemarkCmd : ICommand
	{
		PartyModel model = AmbitionApp.GetModel<PartyModel>();
		public void Execute ()
		{
			if (model.Confidence >= model.ConfidenceCost && model.Hand.Count < 5)
			{
				AmbitionApp.AdjustValue(GameConsts.CONFIDENCE, -10);
				AmbitionApp.SendMessage(PartyMessages.ADD_REMARK);
			}
		}
	}
}