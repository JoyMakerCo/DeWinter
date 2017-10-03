using System;
using Core;

namespace Ambition
{
	public class BuyRemarkCmd : ICommand
	{
		private ModelSvc _models = App.Service<ModelSvc>();
		public void Execute ()
		{
			PartyModel model = _models.GetModel<PartyModel>();
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
