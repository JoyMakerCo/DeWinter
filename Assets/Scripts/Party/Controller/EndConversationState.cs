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
			CommodityVO reward;
            ConversationModel model = _models.GetModel<ConversationModel>();
            int numCharmed = Array.FindAll(model.Guests, g=>g.State == GuestState.Charmed).Length;
			model.Remark = null;
            if (model.Room.Rewards != null)
			{
				int numRewards = model.Room.Rewards.Length;
                reward = model.Room.Rewards[numCharmed < numRewards ? numCharmed : numRewards-1] ;
			}
			else
			{
				reward = GenerateRandomReward(numCharmed, model.Party.Faction);
			}
            model.Room.Cleared = true;

			//Rewards Distributed Here
            if (reward.Type == CommodityType.Servant)
            {
				ServantModel servants = _models.GetModel<ServantModel>();
				if (servants.Servants.ContainsKey(reward.ID))
				{
                    reward = new CommodityVO(CommodityType.Gossip, model.Party.Faction, 1);
                }
            }
            model.Party.Rewards.Add(reward);
            AmbitionApp.OpenDialog("END_CONVERSATION", new CommodityVO[] { reward });
			AmbitionApp.SendMessage(PartyMessages.SHOW_MAP);
		}


		private CommodityVO GenerateRandomReward(int numCharmed, string faction)
    	{
    		int factor = numCharmed < 5 ? numCharmed : 6;
			switch (Util.RNG.Generate(5))
			{
				case 0:
				case 1:
					return new CommodityVO(CommodityType.Reputation, 5*factor);
				case 2:
				case 3:
					return new CommodityVO(CommodityType.Faction, faction, 10*factor);
			}
			return (numCharmed < 5)
				? new CommodityVO(CommodityType.Faction, faction, 1)
				: new CommodityVO(CommodityType.Servant, ServantConsts.SEAMSTRESS, 1);
		}
	}
}
