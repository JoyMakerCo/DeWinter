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
			int numGuests = AmbitionApp.GetModel<MapModel>().Room.Guests.Length;
			while (remarks.Count < model.MaxHandSize)
			{
				remark = new RemarkVO();
				remark.Interest = model.Interests[rnd.Next(model.Interests.Length)];
				remarks.Add(remark);
				remark.Profile = (1+(2*(rnd.Next((int)Math.Pow(2,numGuests-1)))));
	        }
	        model.Hand = remarks;
		}
	}
}