using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class AddRemarkCmd : ICommand
	{
		public void Execute ()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			Random rnd = new Random();
			int numGuests = AmbitionApp.GetModel<MapModel>().Room.Guests.Length;
			string interest = model.Interests[rnd.Next(1, model.Interests.Length)];

			// Create a topic that is exclusive of the previous topic used.
			if (interest == model.LastInterest) interest = model.Interests[0];
			model.LastInterest = interest;

			RemarkVO remark = new RemarkVO(1 + rnd.Next(numGuests), interest);
			model.AddRemark(remark);
		}
	}
}
