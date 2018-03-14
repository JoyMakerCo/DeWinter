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
					guest.Opinion = UnityEngine.Random.Range(stats.Opinion[0], stats.Opinion[1]);
					guest.MaxInterest = UnityEngine.Random.Range(stats.MaxInterest[0], stats.MaxInterest[1]);
					guest.Interest = UnityEngine.Random.Range(stats.Interest[0], stats.Interest[1]);

					guest.Gender = UnityEngine.Random.Range(0,2) == 0 ? Gender.Male : Gender.Female;
					if (guest.Gender == Gender.Female)
			        {
						guest.Title = GetRandomDescriptor("female_title");
						guest.FirstName = GetRandomDescriptor("female_name");
			        }
			        else
			        {
						guest.Title = GetRandomDescriptor("male_title");
						guest.FirstName = GetRandomDescriptor("male_name");
			        }
					guest.LastName = GetRandomDescriptor("last_name");
					guest.LastName  = "aeiouAEIOU".Contains(guest.LastName.Substring(0,1))
						? (" d'" + guest.LastName)
						: (" de " + guest.LastName);

					likeIndex = UnityEngine.Random.Range(0,model.Interests.Length);
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

		private string GetRandomDescriptor(string phrase)
		{
			string [] descriptors = _phrases.GetList(phrase);
			return descriptors[UnityEngine.Random.Range(0, descriptors.Length)];
		}
	}
}
