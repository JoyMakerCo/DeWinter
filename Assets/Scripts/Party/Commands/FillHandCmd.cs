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
			List<RemarkVO> hand = model.Remarks;
			RemarkVO remark;

            int numGuests = model.Guests.Length;
			string interest;
            while (hand.Count < party.MaxHandSize)
			{
                interest = Util.RNG.TakeRandom(party.Interests);
				remark = new RemarkVO(Util.RNG.Generate(1,3), interest);
				hand.Add(remark);
	        }
			model.Remarks = hand;
		}
	}
}
