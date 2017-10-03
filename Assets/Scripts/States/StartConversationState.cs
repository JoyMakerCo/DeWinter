using System;
using UFlow;

namespace Ambition
{
	public class StartConversationState : UState
	{
		public override void OnEnterState ()
		{
			PartyModel pmod = AmbitionApp.GetModel<PartyModel>();
			RoomVO room = AmbitionApp.GetModel<MapModel>().Room;
			GuestVO guest = new GuestVO();
			Random rnd = new Random();
			string name;
			int likeIndex;
			GuestDifficultyVO stats = pmod.GuestDifficultyStats[room.Difficulty-1];

			pmod.Turn = 0;
			pmod.RemarksBought=0;

			if (room.Guests == null)
			{
				// This ensures that previous guest formations stay consistent
				room.Guests = new GuestVO[4]; // TODO: Determine how to vary this number
			}
			for(int i=room.Guests.Length-1; i>=0; i--)
			{
				if (room.Guests[i] == null)
				{
					guest = new GuestVO();
					guest.Opinion = rnd.Next(stats.Opinion[0], stats.Opinion[1]);
					guest.MaxInterest = rnd.Next(stats.MaxInterest[0], stats.MaxInterest[1]);
					guest.Interest = rnd.Next(stats.Interest[0], stats.Interest[1]);

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
			AmbitionApp.SendMessage(PartyMessages.CLEAR_REMARKS);
			AmbitionApp.SendMessage(PartyMessages.FILL_REMARKS);
			AmbitionApp.SendMessage<int>(pmod.Confidence);
		}

		private string GetRandomDescriptor(string [] descriptors, Random rnd)
		{
			return descriptors[rnd.Next(descriptors.Length)];
		}
	}
}
