using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class GrantRemarkCmd : ICommand
	{
		public void Execute ()
		{
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
			RemarkVO[] hand = model.Remarks;
            int index = Array.FindIndex(hand, r => r == null);
            if (index >= 0)
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
                hand[index] = new RemarkVO()
                {
                    NumTargets = Util.RNG.Generate(1, 3),
                    Interest = interest,
                    Free = true
                }; // Assuming 1:2 odds of one target vs two
                model.Remarks=hand;
			}
		}
	}
}
