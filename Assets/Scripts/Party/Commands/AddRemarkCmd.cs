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

				// Create a topic that is exclusive of the previous topic used.
				string interest = model.Interests[UnityEngine.Random.Range(1, model.Interests.Length)];
				model.LastInterest = (interest != model.LastInterest) ? interest : model.Interests[0];

				RemarkVO remark = new RemarkVO(UnityEngine.Random.Range(1,3), interest);
				hand.Add(remark);
				model.Remarks=hand;
			}
		}
	}
}
