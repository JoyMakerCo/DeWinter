using System;
using System.Collections.Generic;
using UFlow;
using Core;

namespace Ambition
{
	public class EndTurnState : UState
	{
		public override void OnEnterState ()
		{
			MapModel map = AmbitionApp.GetModel<MapModel>();
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			RemarkVO remark = model.Remark;
			float levelBonus = (AmbitionApp.GetModel<GameModel>().Level >= 4)
				? 1.25f
				: 1.0f;
			float ReparteBonus = 1.0f + (model.Repartee ? model.ReparteeBonus : 0f);
			foreach (GuestVO guest in map.Room.Guests)
			{
				if (model.TargetedGuests == null || Array.IndexOf(model.TargetedGuests, guest) < 0)
				{
					if (guest.Interest > 0) guest.Interest--;
				}
				else
				{
					guest.Interest = guest.MaxInterest;
				}
				
				if (guest is EnemyVO)
				{
					//Hammering on Offended Guests gives confidence
					// TODO: Configure the spite bonus
					if (guest.State == GuestState.PutOff) model.Confidence += 10;
				}
				else if (model.TargetedGuests != null && !guest.IsLockedIn && Array.IndexOf(model.TargetedGuests, guest) >= 0)
				{
			        //Do they like the Tone?
					if (guest.Like == remark.Interest) //They like the tone
			        {
						guest.Opinion += (int)(Util.RNG.Generate(25,36) * ReparteBonus * levelBonus);
						AmbitionApp.SendMessage(PartyMessages.ADD_REMARK);
						AmbitionApp.GetModel<PartyModel>().Confidence += 5;
					}
					else if (guest.Disike == remark.Interest) //They dislike the tone
			        {
			        	// TODO: All of this needs to be affected by the passive bonus system
			            if (!model.ItemEffect) //If the the Player doesn't have the Fascinator Accessory or its ability has already been used up
			            {
							guest.Opinion -= (int)(Util.RNG.Generate(11,17) * ReparteBonus * levelBonus);
							AmbitionApp.GetModel<PartyModel>().Confidence -= 10;
		            	}
						model.ItemEffect = false;
			        } else //Neutral Tone
			        {
						guest.Opinion += (int)(Util.RNG.Generate(12, 18)*ReparteBonus*levelBonus);
			        }
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
            }

			if (model.Turn%model.FreeRemarkCounter == 0)
				App.Service<MessageSvc>().Send(PartyMessages.ADD_REMARK);
			AmbitionApp.SendMessage<GuestVO[]>(map.Room.Guests);
		}
	}
}
