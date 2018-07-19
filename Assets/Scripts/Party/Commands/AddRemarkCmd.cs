using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class AddRemarkCmd : ICommand
	{
		public void Execute ()
		{
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
			List<RemarkVO> hand = model.Remarks;
            if (hand.Count < AmbitionApp.GetModel<PartyModel>().MaxHandSize)
			{
                string[] interests = AmbitionApp.GetModel<PartyModel>().Interests;
				string interest;

                // Create a topic that is exclusive of the previous topic used.
                if (model.Remark != null)
                {
                    interest = Util.RNG.TakeRandomExcept(interests, model.Remark.Interest);
                }
                else
                {
                    interest = Util.RNG.TakeRandom(interests);
                }

				RemarkVO remark = new RemarkVO(Util.RNG.Generate(1,3), interest);
				hand.Add(remark);
				model.Remarks=hand;
			}
		}
	}
}
