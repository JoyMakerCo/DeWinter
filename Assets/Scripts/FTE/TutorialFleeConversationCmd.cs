using System;
using Core;

namespace Ambition
{
    public class TutorialFleeConversationCmd : ICommand
    {
        public void Execute()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            model.DeckSize = 20;
        }
    }
}
