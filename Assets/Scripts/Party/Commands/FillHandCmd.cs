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
			List<RemarkVO> remarks = model.Hand;
			Random rnd = new Random();
			RemarkVO remark;
			int numGuests = model.Guests.Length;
			while (remarks.Count < model.MaxHandSize)
			{
				remark = new RemarkVO();
				remark.Topic = model.Topics[rnd.Next(model.Topics.Length)];
				remarks.Add(remark);
				remark.Profile = (1+(2*(rnd.Next((int)Math.Pow(2,numGuests-1)))));
	        }
	        model.Hand = remarks;
		}
	}
}