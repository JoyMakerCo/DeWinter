using System;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class AmbushCmd : ICommand<RoomVO>
	{
		public void Execute(RoomVO room)
		{
			EncounterModel emod = DeWinterApp.GetModel<EncounterModel>();
			List<Remark> hand = (emod.PlayerHand.Count > 3) ? emod.PlayerHand.GetRange(0,3) : new List<Remark>(emod.PlayerHand);
			hand.Add(new Remark("ambush", room.Guests.Length));
			hand.Add(new Remark("ambush", room.Guests.Length));
			emod.PlayerHand = hand;
		}
	}
}