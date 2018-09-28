using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class FillHandCmd : ICommand
	{
		public void Execute()
		{
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            PartyModel party = AmbitionApp.GetModel<PartyModel>();
			RemarkVO[] hand = model.Remarks;

            int numGuests = model.Guests.Length;
			string interest;
            for (int i = hand.Length - 1; i >= 0; i--)
			{
                if (hand[i] == null)
                {
                    interest = Util.RNG.TakeRandom(party.Interests);
                    hand[i] = new RemarkVO(Util.RNG.Generate(1, 3), interest);
                }
	        }
			model.Remarks = hand;
		}
	}
}
