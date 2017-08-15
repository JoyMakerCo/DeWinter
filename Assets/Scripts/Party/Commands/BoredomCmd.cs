using System;
using Core;

namespace Ambition
{
	public class BoredomCmd : ICommand
	{
		public void Execute ()
		{
			GuestVO[] guests = AmbitionApp.GetModel<MapModel>().Room.Guests;
			int decay = AmbitionApp.GetModel<PartyModel>().InterestDecay;

			foreach (GuestVO guest in guests)
			{
				guest.Interest-=decay;
			}
			AmbitionApp.GetModel<MapModel>().Room.Guests = guests;
		}
	}
}
