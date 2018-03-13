using System;
using System.Collections.Generic;
using Core;
using UFlow;

namespace Ambition
{
	public class CheckConversationTransition : ULink
	{
		public override bool InitializeAndValidate ()
		{
			MapModel map = AmbitionApp.GetModel<MapModel>();
			GuestVO [] guests = map.Room.Guests;
			int len = guests.Length;
			int numCharmed = Array.FindAll(guests, g=>g.State == GuestState.Charmed).Length;
			int numPutOff = Array.FindAll(guests, g=>g.State == GuestState.PutOff).Length;

			return (numCharmed + numPutOff == len);
		}
	}
}
