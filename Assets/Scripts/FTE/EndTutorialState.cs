using System;
using UFlow;

namespace Ambition
{
    public class EndTutorialState : TutorialState
    {
        public override void OnEnterState()
        {
            base.OnEnterState();
            AmbitionApp.UnregisterCommand<TutorialFleeConversationCmd>(PartyMessages.FLEE_CONVERSATION);
            AmbitionApp.RegisterCommand<FleeConversationCmd>(PartyMessages.FLEE_CONVERSATION);
        }
    }
}
