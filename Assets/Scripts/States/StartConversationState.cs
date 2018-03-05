using System;
using System.Collections.Generic;
using Core;
using UFlow;

namespace Ambition
{
	public class StartConversationState : UState
	{
		private LocalizationModel _phrases;
		public override void OnEnterState ()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			RoomVO room = AmbitionApp.GetModel<MapModel>().Room;
			GuestVO guest = new GuestVO();
			Random rnd = new Random();
			int likeIndex;

			model.Remark = null;
			GuestDifficultyVO stats = model.GuestDifficultyStats[room.Difficulty-1];

			_phrases = AmbitionApp.GetModel<LocalizationModel>();

			model.Turn = 0;
			model.RemarksBought=0;

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

					guest.Gender = rnd.Next(2) == 0 ? Gender.Male : Gender.Female;
					if (guest.Gender == Gender.Female)
			        {
						guest.Title = GetRandomDescriptor("female_title", rnd);
						guest.FirstName = GetRandomDescriptor("female_name", rnd);
			        }
			        else
			        {
						guest.Title = GetRandomDescriptor("male_title", rnd);
						guest.FirstName = GetRandomDescriptor("male_name", rnd);
			        }
					guest.LastName = GetRandomDescriptor("last_name", rnd);
					guest.LastName  = "aeiouAEIOU".Contains(guest.LastName.Substring(0,1))
						? (" d'" + guest.LastName)
						: (" de " + guest.LastName);

					likeIndex = rnd.Next(0,model.Interests.Length);
					guest.Like = model.Interests[likeIndex];
					guest.Disike = model.Interests[(likeIndex + 1)%model.Interests.Length];
					room.Guests[i] = guest;
				}
			}
			AmbitionApp.SendMessage<GuestVO[]>(room.Guests);
			AmbitionApp.SendMessage(PartyMessages.CLEAR_REMARKS);
			AmbitionApp.SendMessage(PartyMessages.FILL_REMARKS);
			AmbitionApp.SendMessage<int>(model.Confidence);
		}

		private string GetRandomDescriptor(string phrase, Random rnd)
		{
			string [] descriptors = _phrases.GetList(phrase);
			return descriptors[rnd.Next(descriptors.Length)];
		}
	}
}
