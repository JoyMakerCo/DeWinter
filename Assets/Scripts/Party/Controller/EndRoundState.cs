using System;
using System.Collections.Generic;
using UFlow;
using Core;

namespace Ambition
{
	public class EndRoundState : UState
	{
		public override void OnEnterState ()
		{
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            foreach (GuestVO g in model.Guests)
            {
                if (g.Opinion >= 100)
                {
                    g.Opinion = 100;
                    g.State = GuestState.Charmed;
                }
                else if (g.Opinion <= 0)
                {
                    g.Opinion = 0;
                    g.State = GuestState.PutOff;
                }
            }
			AmbitionApp.SendMessage(PartyMessages.END_ROUND);
		}
	}
}
