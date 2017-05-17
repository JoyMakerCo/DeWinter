using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class VictoryCheckCmd : ICommand<GuestVO[]>
	{
		public void Execute(GuestVO[] guests)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			int len = guests.Length;
			int numCharmed = Array.FindAll(guests, g=>g.State == GuestState.Charmed).Length;
			int numPutOff = Array.FindAll(guests, g=>g.State == GuestState.PutOff).Length;
			if (numCharmed + numPutOff == len)
			{
				MapModel mmod = AmbitionApp.GetModel<MapModel>();
				RewardVO givenReward = mmod.Room.Rewards[numCharmed]; //Amount of Charmed Guests determines the level of Reward.
				mmod.Room.Cleared = true;

				//Rewards Distributed Here
                if (givenReward.Category == RewardConsts.SERVANT)
                {
					ServantModel smod = AmbitionApp.GetModel<ServantModel>();
					if (smod.Introduced.ContainsKey(givenReward.Type))
					{
                        givenReward = new RewardVO(RewardConsts.GOSSIP, model.Party.faction, 1);
                    }
                }
                model.Rewards.Add(givenReward);

	            Dictionary<string, string> subs = new Dictionary<string, string>(){
					{"$NUMCHARMED",numCharmed.ToString()},
					{"$NUMPUTOFF",numPutOff.ToString()},
					{"$REWARD",givenReward.Name}};
	            AmbitionApp.OpenMessageDialog(DialogConsts.CONVERSATION_OVER_DIALOG, subs);
	            AmbitionApp.CloseDialog(DialogConsts.ENCOUNTER);
    		}
    	}
	}
}