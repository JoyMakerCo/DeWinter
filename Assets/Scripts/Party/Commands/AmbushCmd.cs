using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class AmbushCmd : ICommand<RoomVO>
	{
		public void Execute(RoomVO room)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			List<RemarkVO> hand = model.Remarks;
			string interest;
			RemarkVO remark;
			int numGuests = room.Guests.Length;

			if (hand.Count > model.AmbushHandSize)
				hand.RemoveRange(model.AmbushHandSize, hand.Count - model.AmbushHandSize);
			else while (hand.Count < model.AmbushHandSize)
			{
				interest = model.Interests[UnityEngine.Random.Range(0, model.Interests.Length)];
				remark = new RemarkVO(UnityEngine.Random.Range(1,3), interest);
				hand.Add(remark);
			}
			while(hand.Count < model.MaxHandSize)
				hand.Add(new RemarkVO());

			model.Remarks = hand;

			AmbitionApp.OpenDialog<RoomVO>(DialogConsts.ROOM, room);
			Dictionary<string, string> subs = new Dictionary<string, string>()
				{{"$ROOMNAME", room.Name}};
			AmbitionApp.OpenMessageDialog("ambush_dialog", subs);
		}
	}
}
