using System;
using Core;

namespace Ambition
{
	public class BuyRemarkCmd : ICommand
	{
		PartyModel model = AmbitionApp.GetModel<PartyModel>();
		public void Execute ()
		{
			if (model.RemarksBought < model.ConfidenceCost.Length)
			{
				int cost = model.ConfidenceCost[model.RemarksBought];
				int index = Array.IndexOf(model.Remarks, null);
				if (model.Confidence >= cost && index >= 0)
				{
					AmbitionApp.GetModel<PartyModel>().Confidence -= cost;
					AmbitionApp.SendMessage(PartyMessages.ADD_REMARK);
					model.RemarksBought++;
				}
			}
		}
	}
}
