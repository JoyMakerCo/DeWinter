using System;
using Core;

namespace Ambition
{
	public class BuyRemarkCmd : ICommand
	{
		public void Execute ()
		{
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            PartyModel party = AmbitionApp.GetModel<PartyModel>();
            if (model.Remarks[model.Remarks.Length-1] == null)
			{
                int[] costs = party.ConfidenceCost;
                int cost = model.RemarksBought < costs.Length ? costs[model.RemarksBought] : costs[costs.Length-1];
				if (model.Confidence >= cost)
				{
					model.Confidence -= cost;
					AmbitionApp.SendMessage(PartyMessages.ADD_REMARK);
                    model.RemarksBought++;
				}
			}
		}
	}
}
