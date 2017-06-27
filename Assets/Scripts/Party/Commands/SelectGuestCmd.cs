using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class SelectGuestCmd : ICommand<GuestVO>
	{
		public void Execute (GuestVO guest)
		{
			if (!(guest is EnemyVO) && !guest.IsLockedIn)
			{
				PartyModel model = AmbitionApp.GetModel<PartyModel>();
				Random rnd = new Random();
				int level = AmbitionApp.GetModel<GameModel>().Level;
				float ReparteBonus = model.Repartee ? model.ReparteeBonus : 1.0f;
	        	float opinionMod = 0.0f;

		        //Do they like the Tone?
				if (guest.Like == model.Remark.Interest) //They like the tone
		        {
		            if ((guest is EnemyVO) && level >= 4)
		            {
		            	opinionMod = (float)rnd.Next(25,36) * ReparteBonus * 1.25f;
		            }
		            else
		            {
						opinionMod = (float)rnd.Next(25,36) * ReparteBonus;
		            }
					AmbitionApp.SendMessage(PartyMessages.ADD_REMARK);
		            AmbitionApp.AdjustValue(GameConsts.CONFIDENCE, 5);
				} else if (guest.Disike == model.Remark.Interest) //They dislike the tone
		        {
		        	// TODO: All of this needs to be affected by the passive bonus system
		            if (!model.ItemEffect) //If the the Player doesn't have the Fascinator Accessory or its ability has already been used up
		            {
		                if ((guest is EnemyVO) && level >= 4)
		                {
							opinionMod = (float)-rnd.Next(11,17) * ReparteBonus * 1.25f;
		                }
		                else
		                {
							opinionMod = (float)-rnd.Next(11,17) * ReparteBonus;
		                }
						AmbitionApp.AdjustValue(GameConsts.CONFIDENCE, -10);
	            	}
					model.ItemEffect = false;
		        } else //Neutral Tone
		        {
		            if((guest is EnemyVO) && level >= 4)
		            {
						opinionMod = (float)rnd.Next(12, 18) * ReparteBonus * 1.25f;
		            } else
		            {
						opinionMod = (float)rnd.Next(12, 18) * ReparteBonus;
		            }
		        }
				guest.Opinion += (int)opinionMod;
				switch (guest.State)
				{
					case GuestState.Charmed:
						AmbitionApp.SendMessage(PartyMessages.FILL_REMARKS);
						break;
					case GuestState.PutOff:
						AmbitionApp.AdjustValue(GameConsts.CONFIDENCE, -30);
						break;
				}
			}
			//Hammering on Offended Guests gives confidence
			else if (guest is EnemyVO && guest.State == GuestState.PutOff)
            {
				AmbitionApp.AdjustValue(GameConsts.CONFIDENCE, 10);
            }
		}
	}
}