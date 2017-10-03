using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class SelectGuestCmd : ICommand<GuestVO>
	{
		public void Execute (GuestVO guest)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			MapModel map = AmbitionApp.GetModel<MapModel>();
			RemarkVO remark = model.Remark;
			if (remark == null) return;
			int index = Array.IndexOf(map.Room.Guests, guest);
			if (index < 0) return;
			for (int i=remark.NumTargets-1; i>=0; i--)
			{
				guest = map.Room.Guests[(index + i) % map.Room.Guests.Length];
				
				if (!(guest is EnemyVO) && !guest.IsLockedIn)
				{
					Random rnd = new Random();
					float levelBonus = (AmbitionApp.GetModel<GameModel>().Level >= 4 && guest is EnemyVO)
						? 1.25f
						: 1.0f;
					float ReparteBonus = 1.0f + (model.Repartee ? model.ReparteeBonus : 0f);
		        	float opinionMod = 0.0f;

			        //Do they like the Tone?
					if (guest.Like == remark.Interest) //They like the tone
			        {
						opinionMod = (float)rnd.Next(25,36) * ReparteBonus * levelBonus;
						AmbitionApp.SendMessage(PartyMessages.ADD_REMARK);
						AmbitionApp.GetModel<PartyModel>().Confidence += 5;
					} else if (guest.Disike == remark.Interest) //They dislike the tone
			        {
			        	// TODO: All of this needs to be affected by the passive bonus system
			            if (!model.ItemEffect) //If the the Player doesn't have the Fascinator Accessory or its ability has already been used up
			            {
							opinionMod = -(float)rnd.Next(11,17) * ReparteBonus * levelBonus;
							AmbitionApp.GetModel<PartyModel>().Confidence -= 10;
		            	}
						model.ItemEffect = false;
			        } else //Neutral Tone
			        {
						opinionMod = (float)rnd.Next(12, 18)*ReparteBonus*levelBonus;
			        }
					guest.Opinion += (int)opinionMod;
					switch (guest.State)
					{
						case GuestState.Charmed:
							AmbitionApp.SendMessage(PartyMessages.FILL_REMARKS);
							break;
						case GuestState.PutOff:
							AmbitionApp.GetModel<PartyModel>().Confidence -= 30;
							break;
					}
				}
				//Hammering on Offended Guests gives confidence
				else if (guest is EnemyVO && guest.State == GuestState.PutOff)
	            {
					AmbitionApp.GetModel<PartyModel>().Confidence += 10;
	            }
				guest.Interest = guest.MaxInterest+1;
            }
            remark = null;
            AmbitionApp.SendMessage(PartyMessages.END_TURN);
		}
	}
}
