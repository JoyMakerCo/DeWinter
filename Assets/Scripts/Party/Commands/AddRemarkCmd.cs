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
			List<RemarkVO> hand = model.Remarks;
			int length = model.IsAmbush ? model.AmbushHandSize : model.MaxHandSize;
			if (hand.Count < length)
			{
				int numGuests = AmbitionApp.GetModel<MapModel>().Room.Guests.Length;
				string interest;

				// Create a topic that is exclusive of the previous topic used.
				if (model.Remark != null)
				{
					interest = model.Interests[Util.RNG.Generate(1, model.Interests.Length)];
					if (model.Remark.Interest == interest) interest = model.Interests[0];
				}
				else
				{
					interest = model.Interests[Util.RNG.Generate(0, model.Interests.Length)];
				}

				RemarkVO remark = new RemarkVO(Util.RNG.Generate(1,3), interest);
				hand.Add(remark);
				model.Remarks=hand;
			}
		}
	}
}
