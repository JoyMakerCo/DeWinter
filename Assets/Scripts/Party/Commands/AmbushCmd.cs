using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class AmbushCmd : ICommand<RoomVO>
	{
		public void Execute(RoomVO room)
		{
            PartyModel party = AmbitionApp.GetModel<PartyModel>();
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
			List<RemarkVO> hand = model.Remarks;
			string interest;
			RemarkVO remark;
            int numGuests = model.Guests.Length;

            if (hand.Count > party.AmbushHandSize)
                hand.RemoveRange(party.AmbushHandSize, hand.Count - party.AmbushHandSize);
            else while (hand.Count < party.AmbushHandSize)
			{
                interest = Util.RNG.TakeRandom(party.Interests);
				remark = new RemarkVO(Util.RNG.Generate(1,3), interest);
				hand.Add(remark);
			}
            while(hand.Count < party.MaxHandSize)
				hand.Add(new RemarkVO());

			model.Remarks = hand;

			AmbitionApp.OpenDialog<RoomVO>(DialogConsts.ROOM, room);
			Dictionary<string, string> subs = new Dictionary<string, string>()
				{{"$ROOMNAME", room.Name}};
			AmbitionApp.OpenMessageDialog("ambush_dialog", subs);
		}
	}
}
