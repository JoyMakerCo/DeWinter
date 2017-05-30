using System;
using Core;

namespace Ambition
{
	public class MakeRandomGuestCmd : ICommand
	{
		public void Execute()
		{
			PartyModel pmod = AmbitionApp.GetModel<PartyModel>();
			PartyVO party = pmod.Party;
			if (party == null) return; // There's no guests if there's no party

			GuestVO guest = new GuestVO();
			Random rnd = new Random();
			string name;
			int likeIndex;

			guest.IsFemale = rnd.Next((int)(party.MaleToFemaleRatio*100f) + 100) >= 100*party.MaleToFemaleRatio;
			if (guest.IsFemale)
	        {
				name = GetRandomDescriptor(pmod.FemaleTitles, rnd);
				name += " " + GetRandomDescriptor(pmod.FemaleNames, rnd);
				name += " de " + GetRandomDescriptor(pmod.LastNames, rnd);
	        }
	        else
	        {
				name = GetRandomDescriptor(pmod.MaleTitles, rnd);
				name += " " + GetRandomDescriptor(pmod.MaleNames, rnd);
				name += " de " + GetRandomDescriptor(pmod.LastNames, rnd);
	        }
			guest.Name = name;

			likeIndex = rnd.Next(0,pmod.Interests.Length);
			guest.Like = pmod.Interests[likeIndex];
			guest.Disike = pmod.Interests[(likeIndex + 1)%pmod.Interests.Length];
			guest.Opinion = rnd.Next(25,51);
			AmbitionApp.SendMessage<GuestVO>(guest);
		}

		private string GetRandomDescriptor(string [] descriptors, Random rnd)
		{
			return descriptors[rnd.Next(descriptors.Length)];
		}
	}
}