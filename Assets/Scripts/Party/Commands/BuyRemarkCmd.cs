using System;
using Core;

namespace Ambition
{
	public class BuyRemarkCmd : ICommand
	{
		PartyModel model = AmbitionApp.GetModel<PartyModel>();
		public void Execute ()
		{
//			if (model.Confidence >= model.ConfidenceCost)
			{
				int index = Array.IndexOf(model.Remarks, null);
				if (index >= 0)
				{
					AmbitionApp.AdjustValue(GameConsts.CONFIDENCE, -10);
					AmbitionApp.SendMessage(PartyMessages.ADD_REMARK);
				}
			}
		}
	}
}
