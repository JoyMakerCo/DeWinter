using System;
using Core;
using UFlow;

namespace Ambition
{
	public class StartConversationState : UState
	{
		private LocalizationModel _phrases;
		public override void OnEnterState ()
		{
			PartyModel pmod = AmbitionApp.GetModel<PartyModel>();
			RoomVO room = AmbitionApp.GetModel<MapModel>().Room;
			GuestVO guest = new GuestVO();
			Random rnd = new Random();
			string title;
			string name;
			string lname;
			int likeIndex;
			GuestDifficultyVO stats = pmod.GuestDifficultyStats[room.Difficulty-1];

			_phrases = AmbitionApp.GetModel<LocalizationModel>();

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
						title = GetRandomDescriptor("female_title", rnd);
						name = GetRandomDescriptor("female_name", rnd);
			        }
			        else
			        {
						title = GetRandomDescriptor("male_title", rnd);
						name = GetRandomDescriptor("male_name", rnd);
			        }
					lname = "de " + GetRandomDescriptor("last_name", rnd);
					guest.Name = title + " " + name + " " + lname;
					guest.DisplayName = title + " " + lname;

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

		private string GetRandomDescriptor(string phrase, Random rnd)
		{
			string [] descriptors = _phrases.GetList(phrase);
			return descriptors[rnd.Next(descriptors.Length)];
		}
	}
}
