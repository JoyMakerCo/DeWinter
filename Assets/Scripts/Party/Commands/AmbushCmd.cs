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
			RemarkVO[] hand = model.Remarks;
			string interest;
            int numGuests = model.Guests.Length;
            int i;
            for (i = hand.Length-1; i >= party.AmbushHandSize-1; i--)
            {
                hand[i] = new RemarkVO();
            }
            while (hand[i] == null)
            {
                interest = Util.RNG.TakeRandom(party.Interests);
                hand[i] = new RemarkVO(Util.RNG.Generate(1, 3), interest);
                i--;
            }

			model.Remarks = hand;

			AmbitionApp.OpenDialog<RoomVO>(DialogConsts.ROOM, room);
			Dictionary<string, string> subs = new Dictionary<string, string>()
				{{"$ROOMNAME", room.Name}};
			AmbitionApp.OpenMessageDialog("ambush_dialog", subs);
		}
	}
}
