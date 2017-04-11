using System;
using Core;

namespace DeWinter
{
	public class MakeRandomGuestCmd : ICommand
	{
		public void Execute()
		{
			PartyModel pmod = DeWinterApp.GetModel<PartyModel>();
			Party party = pmod.Party;
			if (party == null) return; // There's no guests if there's no party

			GuestVO guest = new GuestVO();
			Random rnd = new Random();
			string name;

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
	        guest.disposition = rnd.Next(0,4);
			guest.Opinion = Random.Range(25,51);
			DeWinterApp.SendMessage<GuestVO>(guest);
		}

		private string GetRandomDescriptor(string [] descriptors, Random rnd)
		{
			return descriptors[rnd.Next(descriptors.Length)];
		}
	}
}