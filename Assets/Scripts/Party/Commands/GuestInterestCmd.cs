using System;
using Core;

namespace Ambition
{
	public class GuestInterestCmd : ICommand<GuestVO>
	{
		public void Execute (GuestVO guest)
		{
			RemarkVO remark = AmbitionApp.GetModel<PartyModel>().Remark;
			GuestVO[] guests = AmbitionApp.GetModel<MapModel>().Room.Guests;
			int index = Array.IndexOf(guests, guest);
			if (remark == null || index < 0) return;

			int decay = AmbitionApp.GetModel<PartyModel>().InterestDecay;
			for (int i=guests.Length-1; i>=0; i--)
			{
				if (i > remark.NumTargets)
					guests[(i+index)%remark.NumTargets].Interest-=decay;
				else
					guests[(i+index)%remark.NumTargets].Interest = 100;
			}
		}
	}
}
