using System;
using Core;

namespace Ambition
{
	public class BuyRemarkCmd : ICommand
	{
		PartyModel model = AmbitionApp.GetModel<PartyModel>();
		public void Execute ()
		{
			if (model.Confidence >= model.ConfidenceCost)
			{
				for(int i=0; i<model.Remarks.Length; i++)
				{
					if (model.Remarks[i] == null)
					{
						AmbitionApp.AdjustValue(GameConsts.CONFIDENCE, -10);
						AmbitionApp.SendMessage(PartyMessages.ADD_REMARK);
						return;
					}
				}
			}
		}
	}
}