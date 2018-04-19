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
			if (model.Remarks.Count < model.MaxHandSize)
			{
				int cost = model.ConfidenceCost[model.RemarksBought];
				if (model.Confidence >= cost)
				{
					AmbitionApp.GetModel<PartyModel>().Confidence -= cost;
					AmbitionApp.SendMessage(PartyMessages.ADD_REMARK);
					model.RemarksBought++;
				}
			}
		}
	}
}
