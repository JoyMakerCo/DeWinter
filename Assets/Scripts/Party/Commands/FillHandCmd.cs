﻿using System;
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

			int numGuests = AmbitionApp.GetModel<MapModel>().Room.Guests.Length;
			string interest;
			while (hand.Count < model.MaxHandSize)
			{
				interest = model.Interests[Util.RNG.Generate(0, model.Interests.Length)];
				remark = new RemarkVO(Util.RNG.Generate(1,3), interest);
				hand.Add(remark);
	        }
			model.Remarks = hand;
		}
	}
}
