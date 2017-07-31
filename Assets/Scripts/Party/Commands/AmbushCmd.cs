using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class AmbushCmd : ICommand<RoomVO>
	{
		public void Execute(RoomVO room)
		{
			PartyModel pmod = AmbitionApp.GetModel<PartyModel>();
			RemarkVO[] hand = pmod.Remarks;
			for (int i=hand.Length-1; i>=pmod.AmbushHandSize; i--)
			{
				hand[i] = new RemarkVO();
			}
			pmod.Remarks = hand;

			AmbitionApp.OpenDialog<RoomVO>(DialogConsts.ROOM, room);

			Dictionary<string, string> subs = new Dictionary<string, string>()
				{{"$ROOMNAME", room.Name}};
			AmbitionApp.OpenMessageDialog("ambush_dialog", subs);
		}
	}
}