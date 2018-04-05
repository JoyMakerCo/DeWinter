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
			RoomVO room = AmbitionApp.GetModel<MapModel>().Room;
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			model.Turn = 0;
			model.Remark = null;
			_phrases = AmbitionApp.GetModel<LocalizationModel>();
			AmbitionApp.SendMessage(PartyMessages.CLEAR_REMARKS);

			// This ensures that previous guest formations stay consistent
			if (room.Guests == null)
			{
				GuestVO guest;
				room.Guests = new GuestVO[4]; // TODO: Determine how to vary this number
				for (int i=room.Guests.Length-1; i>=0; i--)
				{
					guest = new GuestVO();
					if (Util.RNG.Generate(2)==0)
					{
							guest.Gender = Gender.Female;
							guest.Title = GetRandomDescriptor("female_title");
							guest.FirstName = GetRandomDescriptor("female_name");
					}
					else
					{
							guest.Gender = Gender.Male;
							guest.Title = GetRandomDescriptor("male_title");
							guest.FirstName = GetRandomDescriptor("male_name");
					}
					guest.LastName = GetRandomDescriptor("last_name");
					guest.LastName  = "aeiouAEIOU".Contains(guest.LastName.Substring(0,1))
						? (" d'" + guest.LastName)
						: (" de " + guest.LastName);
					room.Guests[i] = guest;
				}
			}
			if (!room.Cleared)
			{
				int likeIndex;
				GuestDifficultyVO stats = model.GuestDifficultyStats[room.Difficulty-1];
				model.RemarksBought=0;
				foreach (GuestVO guest in room.Guests)
				{
					guest.Opinion = Util.RNG.Generate(stats.Opinion[0], stats.Opinion[1]);
					guest.MaxInterest = Util.RNG.Generate(stats.MaxInterest[0], stats.MaxInterest[1]);
					guest.Interest = Util.RNG.Generate(stats.Interest[0], stats.Interest[1]);
					likeIndex = Util.RNG.Generate(0,model.Interests.Length);
					guest.Like = model.Interests[likeIndex];
					guest.Dislike = model.Interests[(likeIndex + 1)%model.Interests.Length];
				}
			}
			AmbitionApp.SendMessage<GuestVO[]>(room.Guests);
			AmbitionApp.SendMessage<int>(model.Confidence);
		}

		private string GetRandomDescriptor(string phrase)
		{
			string [] descriptors = _phrases.GetList(phrase);
			return descriptors[Util.RNG.Generate(0, descriptors.Length)];
		}
	}
}
