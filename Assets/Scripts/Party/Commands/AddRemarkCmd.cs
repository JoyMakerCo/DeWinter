using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class AddRemarkCmd : ICommand
	{
		public void Execute ()
		{
			PartyModel pmod = AmbitionApp.GetModel<PartyModel>();
			RemarkVO remark = new RemarkVO();

			// Create a topic that is exclusive of the previous topic used.
			Random rnd = new Random();
			remark.Topic = pmod.Topics[rnd.Next(1, pmod.Topics.Length)];
			if (remark.Topic == pmod.LastTopic) remark.Topic = pmod.Topics[0];
			pmod.LastTopic = remark.Topic;

			// Generate a targeting profile
			// Profile is an integer acting as a bit array, which always has a leading and trailing bit
			int numGuests = pmod.Guests.Length;
			remark.Profile = (1+(2*(rnd.Next((int)Math.Pow(2,numGuests-1)))));
			pmod.AddRemark(remark);
		}
	}
}