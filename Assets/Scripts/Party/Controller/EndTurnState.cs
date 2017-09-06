using System;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
	public class EndTurnState : UState
	{
		public override void OnEnterState ()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			MapModel map = AmbitionApp.GetModel<MapModel>();
			GuestVO [] guests = map.Room.Guests;
			int len = guests.Length;
			int numCharmed = Array.FindAll(guests, g=>g.State == GuestState.Charmed).Length;
			int numPutOff = Array.FindAll(guests, g=>g.State == GuestState.PutOff).Length;

			model.Remark = null;

			if (numCharmed + numPutOff == len)
			{
				RewardVO reward;
				if (map.Room.Rewards != null)
				{
					int numRewards = map.Room.Rewards.Length;
					reward = map.Room.Rewards[numCharmed < numRewards ? numCharmed : numRewards-1] ;
				}
				else
				{
					reward = GenerateRandomReward(numCharmed, model.Party.Faction);
				}
				map.Room.Cleared = true;

				//Rewards Distributed Here
                if (reward.Category == RewardConsts.SERVANT)
                {
					ServantModel smod = AmbitionApp.GetModel<ServantModel>();
					if (smod.Introduced.ContainsKey(reward.Type))
					{
                        reward = new RewardVO(RewardConsts.GOSSIP, model.Party.Faction, 1);
                    }
                }
                model.Party.Rewards.Add(reward);

	            Dictionary<string, string> subs = new Dictionary<string, string>(){
					{"$NUMCHARMED",numCharmed.ToString()},
					{"$NUMPUTOFF",numPutOff.ToString()},
					{"$REWARD",reward.Name}};
	            AmbitionApp.OpenMessageDialog(DialogConsts.CONVERSATION_OVER_DIALOG, subs);
				AmbitionApp.SendMessage(PartyMessages.SHOW_MAP);
    		}
			AmbitionApp.SendMessage<GuestVO[]>(AmbitionApp.GetModel<MapModel>().Room.Guests);
		}

		private RewardVO GenerateRandomReward(int numCharmed, string faction)
    	{
    		int factor = numCharmed < 5 ? numCharmed : 6;
			switch (new Random().Next(5))
			{
				case 0:
				case 1:
					return new RewardVO(RewardConsts.VALUE, GameConsts.REPUTATION, 5*factor);
				case 2:
				case 3:
					return new RewardVO(RewardConsts.FACTION, faction, 10*factor);
			}
			return (numCharmed < 5)
				? new RewardVO(RewardConsts.GOSSIP, faction, 1)
				: new RewardVO(RewardConsts.SERVANT, ServantConsts.SEAMSTRESS, 1);
		}
	}
}
