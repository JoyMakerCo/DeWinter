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
			RemarkVO remark = new RemarkVO();

			// Create a topic that is exclusive of the previous topic used.
			Random rnd = new Random();
			remark.Interest = model.Interests[rnd.Next(1, model.Interests.Length)];
			if (remark.Interest == model.LastInterest) remark.Interest = model.Interests[0];
			model.LastInterest = remark.Interest;

			// Generate a targeting profile
			// Profile is an integer acting as a bit array, which always has a leading and trailing bit
			int numGuests = AmbitionApp.GetModel<MapModel>().Room.Guests.Length;
			remark.Profile = (1+(2*(rnd.Next((int)Math.Pow(2,numGuests-1)))));
			model.AddRemark(remark);
		}
	}
}