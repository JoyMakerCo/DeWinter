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
			RemarkVO[] remarks = model.Remarks;
			Random rnd = new Random();
			int numGuests = AmbitionApp.GetModel<MapModel>().Room.Guests.Length;
			string interest;
			for (int i=remarks.Length-1; i>=0; i--)
			{
				if (remarks[i]==null)
				{
					interest = model.Interests[rnd.Next(model.Interests.Length)];
					remarks[i] = new RemarkVO(1 + rnd.Next(numGuests), interest);
				}
	        }
	        model.Remarks = remarks;
		}
	}
}
