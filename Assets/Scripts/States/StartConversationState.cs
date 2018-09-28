using System;
using System.Collections.Generic;
using Core;
using Util;
using UFlow;

namespace Ambition
{
	public class StartConversationState : UState
	{
		private LocalizationModel _phrases;
		public override void OnEnterState ()
		{
            PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
			ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
			GuestVO guest;
            GuestVO[] guests = model.Guests;
            _phrases = AmbitionApp.GetModel<LocalizationModel>();

            // This ensures that previous guest formations stay consistent
            // TODO: Determine how to vary this number
            if (guests == null) guests = new GuestVO[4];
            for (int i = guests.Length - 1; i >= 0; i--)
            {
                if (guests[i] == null)
                {
                    guest = new GuestVO();
                    if (RNG.Generate(2) == 0)
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
                    guest.LastName = "aeiouAEIOU".Contains(guest.LastName.Substring(0, 1))
                        ? (" d'" + guest.LastName)
                        : (" de " + guest.LastName);
                    guests[i] = guest;
                }
            }

            RoomVO room = model.Room;
			if (!room.Cleared)
			{
				int likeIndex;
                GuestDifficultyVO stats = partyModel.GuestDifficultyStats[room.Difficulty-1];
                string[] interests = partyModel.Interests;
                foreach (GuestVO g in guests)
				{
					g.Opinion = RNG.Generate(stats.Opinion[0], stats.Opinion[1]);
					g.MaxInterest = RNG.Generate(stats.MaxInterest[0], stats.MaxInterest[1]);
					g.Interest = RNG.Generate(stats.Interest[0], stats.Interest[1]);
                    likeIndex = RNG.Generate(interests.Length);
                    g.Like = interests[likeIndex];
					g.Dislike = interests[(likeIndex + 1)%interests.Length];
				}
				// All Variety of Likes final check
                if (Array.TrueForAll(guests, g=>g.Like == guests[0].Like))
				{
                    guest = RNG.TakeRandom(guests);
                    likeIndex = RNG.Generate(interests.Length);
                    guest.Like = interests[likeIndex];
                    guest.Dislike = interests[(likeIndex + 1) % interests.Length];
				}
			}
            model.Guests = guests;
            model.Round = 0;
            model.Remark = null;
            model.FreeRemarkCounter = partyModel.FreeRemarkCounter;
            model.Repartee = false;
            model.RemarksBought = 0;
            AmbitionApp.SendMessage<int>(GameConsts.CONFIDENCE, partyModel.Confidence);
		}

        private string GetRandomDescriptor(string phrase)
        {
            string[] descriptors = _phrases.GetList(phrase);
            return RNG.TakeRandom(descriptors);
        }
	}
}
