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
            //foreach (CharacterVO g in model.Guests)
            //{
            //    if (g.State == GuestState.Bored || g.State == GuestState.PutOff) //You can't have a guest that's not both Put Off and bored, since negative remarks make guests bored, but you can have a guest that is bored, but not yet put off
            //    {
            //        g.Opinion -= partyModel.BoredomPenalty;
            //        if (g.Opinion <= 0)
            //        {
            //            g.Opinion = 0;
            //            AmbitionApp.SendMessage(PartyMessages.GUEST_OFFENDED, g);
            //        }
            //        AmbitionApp.SendMessage(PartyMessages.BURN_REMARKS, 1);
            //        AmbitionApp.SendMessage(PartyMessages.GUEST_REACTION_BORED, g);
            //    }
            //    AmbitionApp.SendMessage(g);
            //}
        }
    }
}
