using System;
using System.Collections.Generic;
using UFlow;
using Core;

namespace Ambition
{
	public class EndConversationState : UState
	{
		private ModelSvc _models = App.Service<ModelSvc>();

		public override void OnEnterState ()
		{
			RewardVO reward;
			PartyModel model = _models.GetModel<PartyModel>();
			MapModel map = _models.GetModel<MapModel>();
			int numCharmed = Array.FindAll(map.Room.Guests, g=>g.State == GuestState.Charmed).Length;
			model.Remark = null;
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
				ServantModel servants = _models.GetModel<ServantModel>();
				if (servants.Servants.ContainsKey(reward.Type))
				{
                    reward = new RewardVO(RewardConsts.GOSSIP, model.Party.Faction, 1);
                }
            }
            model.Party.Rewards.Add(reward);
			Dictionary<string, string> subs = new Dictionary<string, string>(){
				{"$NUMCHARMED",numCharmed.ToString()},
				{"$NUMPUTOFF",(map.Room.Guests.Length - numCharmed).ToString()},
				{"$REWARD",reward.Name}};
            AmbitionApp.OpenMessageDialog(DialogConsts.CONVERSATION_OVER_DIALOG, subs);

            // TODO: This belongs in a Party-Wide State Machine

			if (Array.Exists(map.Map.Rooms,r=>!r.Cleared))
            {
				AmbitionApp.SendMessage(PartyMessages.SHOW_MAP);
            }
            else
            {
				AmbitionApp.SendMessage(PartyMessages.END_PARTY);
			}
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
