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
            ConversationModel model = _models.GetModel<ConversationModel>();
            int numCharmed = Array.FindAll(model.Guests, g=>g.State == GuestState.Charmed).Length;
			model.Remark = null;
            model.Room.Cleared = true;

            if (model.Room.Rewards != null && model.Room.Rewards.Length > 0)
            {
                model.Party.Rewards.AddRange(model.Room.Rewards);
                AmbitionApp.SendMessage(model.Room.Rewards);
            }
            else
            {
                model.Party.Rewards.Add(GenerateRandomReward(numCharmed, model.Party.Faction));
            }
            int numRemarks = (int)(model.MaxDeckSize * .1f);
            AmbitionApp.SendMessage(PartyMessages.RESHUFFLE_REMARKS, numRemarks);
            //AmbitionApp.OpenDialog("END_CONVERSATION", model.Room.Rewards);
			AmbitionApp.SendMessage(PartyMessages.END_CONVERSATION);
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
                    return new CommodityVO(CommodityType.Reputation, faction, 10*factor);
			}
			return (numCharmed < 5)
                ? new CommodityVO(CommodityType.Reputation, faction, 1)
				: new CommodityVO(CommodityType.Servant, ServantConsts.SEAMSTRESS, 1);
		}
	}
}
