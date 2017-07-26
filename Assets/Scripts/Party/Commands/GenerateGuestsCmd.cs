using System;
using Core;

namespace Ambition
{
	public class GenerateGuestsCmd : ICommand<RoomVO>
	{
		/// <summary>
		/// Replaces each null occurrence within room.Guests
		/// with an apporpriate guest.
		/// </summary>
		/// <param name="room">Room.</param>
		public void Execute (RoomVO room)
		{
			PartyModel pmod = AmbitionApp.GetModel<PartyModel>();
			GuestVO guest = new GuestVO();
			Random rnd = new Random();
			string name;
			int likeIndex;

			if (room.Guests == null)
			{
				room.Guests = new GuestVO[4]; // TODO: Determine how to vary this number
			}
			for(int i=room.Guests.Length-1; i>=0; i--)
			{
				if (room.Guests[i] == null)
				{
					guest = new GuestVO();
					switch (room.Difficulty)
			        {
			            case 1:
							guest.Opinion = rnd.Next(25,51);
							break;
						case 2:
							guest.Opinion = rnd.Next(25,46);
							break;
						case 3:
							guest.Opinion = rnd.Next(25,41);
							break;
		    	        case 4:
							guest.Opinion = rnd.Next(25,36);
							break;
		    		    case 5:
							guest.Opinion = rnd.Next(20,31);
							break;
					}

					guest.IsFemale = rnd.Next((int)(pmod.Party.MaleToFemaleRatio*100f) + 100) >= 100*pmod.Party.MaleToFemaleRatio;
					if (guest.IsFemale)
			        {
						name = GetRandomDescriptor(pmod.FemaleTitles, rnd);
						name += " " + GetRandomDescriptor(pmod.FemaleNames, rnd);
			        }
			        else
			        {
						name = GetRandomDescriptor(pmod.MaleTitles, rnd);
						name += " " + GetRandomDescriptor(pmod.MaleNames, rnd);
			        }
					guest.Name = name + " de " + GetRandomDescriptor(pmod.LastNames, rnd);

					likeIndex = rnd.Next(0,pmod.Interests.Length);
					guest.Like = pmod.Interests[likeIndex];
					guest.Disike = pmod.Interests[(likeIndex + 1)%pmod.Interests.Length];
					room.Guests[i] = guest;
				}
			}
			AmbitionApp.SendMessage<GuestVO[]>(room.Guests);
		}

		private string GetRandomDescriptor(string [] descriptors, Random rnd)
		{
			return descriptors[rnd.Next(descriptors.Length)];
		}
	}
}
