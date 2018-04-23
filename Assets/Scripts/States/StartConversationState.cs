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
			GuestVO guest;

			model.Turn = 0;
			model.Remark = null;
			_phrases = AmbitionApp.GetModel<LocalizationModel>();
			AmbitionApp.SendMessage(PartyMessages.CLEAR_REMARKS);

			// This ensures that previous guest formations stay consistent
			// TODO: Determine how to vary this number
			if (room.Guests == null) room.Guests = new GuestVO[4];
			for (int i=room.Guests.Length-1; i>=0; i--)
			{
				if (room.Guests[i] == null)
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
				foreach (GuestVO g in room.Guests)
				{
					g.Opinion = Util.RNG.Generate(stats.Opinion[0], stats.Opinion[1]);
					g.MaxInterest = Util.RNG.Generate(stats.MaxInterest[0], stats.MaxInterest[1]);
					g.Interest = Util.RNG.Generate(stats.Interest[0], stats.Interest[1]);
					likeIndex = Util.RNG.Generate(0,model.Interests.Length);
					g.Like = model.Interests[likeIndex];
					g.Dislike = model.Interests[(likeIndex + 1)%model.Interests.Length];
				}
				// All Variety of Likes final check
				if (Array.TrueForAll(room.Guests, g=>g.Like == room.Guests[0].Like))
				{
					GuestVO g = room.Guests[Util.RNG.Generate(room.Guests.Length)];
					likeIndex = Util.RNG.Generate(1,model.Interests.Length);
					if (model.Interests[likeIndex] == g.Like) likeIndex = 0;
					g.Like = model.Interests[likeIndex];
					g.Dislike = model.Interests[(likeIndex + 1)%model.Interests.Length];
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
