﻿using UFlow;

namespace Ambition
{
    public class DrinkState : UState
    {
        public override void OnEnterState()
        {
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			if (model.Drink > 0)
	        {
                AmbitionApp.SendMessage(PartyMessages.DRAW_REMARK);
 				model.Drink--;
                model.Intoxication++;
                AmbitionApp.GetModel<ConversationModel>().Remark = null;
            }
        }
	}
}
