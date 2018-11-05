using System;
using UFlow;

namespace Ambition
{
    public class UpdateGuestsState : UState
    {
        public override void OnEnterState()
        {
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
            foreach (GuestVO g in model.Guests)
            {
                if (g.State == GuestState.Bored)
                {
                    g.Opinion -= partyModel.BoredomPenalty;
                    if (g.Opinion <= 0)
                    {
                        g.Opinion = 0;
                        g.State = GuestState.PutOff;
                    }
                    AmbitionApp.SendMessage(PartyMessages.BURN_REMARKS, 1);
                }
                AmbitionApp.SendMessage(g);
            }
        }
    }
}
