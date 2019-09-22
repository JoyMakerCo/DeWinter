using System;
using System.Collections.Generic;
using Core;
using Util;
using UFlow;

namespace Ambition
{
	public class InitConversationCmd : ICommand
	{
		private LocalizationSvc _phrases;
		public void Execute ()
		{
/*
            PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
            RoomVO room = AmbitionApp.GetModel<MapModel>().Room;
            CharacterVO[] guests = room?.Guests;
            CharacterVO guest;

            AmbitionApp.RegisterModel<ConversationModel>();

            _phrases = AmbitionApp.GetModel<LocalizationModel>();

            // This ensures that previous guest formations stay consistent
            if (guests == null || guests.Length == 0)
            {
                // TODO: Determine how to vary this number
                guests = new CharacterVO[4];
            }
            for (int i = guests.Length - 1; i >= 0; i--)
            {
                if (guests[i] == null)
                {
                    guest = new CharacterVO();
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

 			if (!room.Cleared)
			{
				int likeIndex;
                GuestDifficultyVO stats = partyModel.GuestDifficultyStats[0room.Difficulty-1];
                string[] interests = partyModel.Interests;

                if (room.Actions != null)
                    AmbitionApp.SendMessage(room.Actions);

                foreach (CharacterVO g in guests)
				{
					g.Opinion = RNG.Generate(stats.Opinion[0], stats.Opinion[1]);
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
            room.Guests = guests;
            AmbitionApp.SendMessage(guests);
*/
        }

        private string GetRandomDescriptor(string phrase)
        {
            string[] descriptors = _phrases.GetList(phrase);
            return RNG.TakeRandom(descriptors);
        }
	}
}
