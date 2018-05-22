using System;
using System.Collections.Generic;
using Core;
using UFlow;

namespace Ambition
{
	public class CheckConversationTransition : ULink
	{
		public override void Initialize ()
		{
			MapModel map = AmbitionApp.GetModel<MapModel>();
			GuestVO [] guests = map.Room.Guests;
			int numCharmed = Array.FindAll(guests, g=>g.State == GuestState.Charmed).Length;
			int numPutOff = Array.FindAll(guests, g=>g.State == GuestState.PutOff).Length;
			
			if (numCharmed + numPutOff == guests.Length) Activate();
		}
	}
}
