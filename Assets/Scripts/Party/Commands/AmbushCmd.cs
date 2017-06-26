using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class AmbushCmd : ICommand<RoomVO>
	{
		public void Execute(RoomVO room)
		{
			// TODO: Is this how ambush works??
			PartyModel pmod = AmbitionApp.GetModel<PartyModel>();
			List<RemarkVO> hand = (pmod.Hand.Count > 3) ? pmod.Hand.GetRange(0,3) : pmod.Hand;
			hand.Add(new RemarkVO()); // Uninitialized remarks are Ambush remarks!
			hand.Add(new RemarkVO());
			pmod.Hand = hand;

			AmbitionApp.OpenDialog<RoomVO>(DialogConsts.ROOM, room);

			Dictionary<string, string> subs = new Dictionary<string, string>()
				{{"$ROOMNAME", room.Name}};
			AmbitionApp.OpenMessageDialog("ambush_dialog", subs);
		}
	}
}