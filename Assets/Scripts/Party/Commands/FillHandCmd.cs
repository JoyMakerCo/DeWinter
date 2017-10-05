using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class FillHandCmd : ICommand
	{
		public void Execute()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			List<RemarkVO> hand = model.Remarks;
			RemarkVO remark;
			Random rnd = new Random();

			int numGuests = AmbitionApp.GetModel<MapModel>().Room.Guests.Length;
			string interest;
			while (hand.Count < model.MaxHandSize)
			{
				interest = model.Interests[rnd.Next(model.Interests.Length)];
				remark = new RemarkVO(1 + rnd.Next(numGuests), interest);
				hand.Add(remark);
	        }
			model.Remarks = hand;
		}
	}
}
