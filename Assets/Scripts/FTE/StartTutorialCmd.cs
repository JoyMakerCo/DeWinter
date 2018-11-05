using Core;

namespace Ambition
{
    public class StartTutorialCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.UnregisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
            AmbitionApp.UnregisterCommand<SkipTutorialCmd>(GameMessages.SKIP_TUTORIAL);
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            model.DeckSize = 20;

            AmbitionApp.RegisterCommand<TutorialFleeConversationCmd>(PartyMessages.FLEE_CONVERSATION);

            AmbitionApp.RegisterState(TutorialConsts.TUTORIAL_MACHINE, "TutorialStart");
            AmbitionApp.RegisterState<TutorialState>(TutorialConsts.TUTORIAL_MACHINE, "ShowTutorialView");
            AmbitionApp.RegisterState<EndTutorialState>(TutorialConsts.TUTORIAL_MACHINE, "EndTutorialParty");

            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>(TutorialConsts.TUTORIAL_MACHINE, "TutorialStart", "ShowTutorialView", PartyMessages.SHOW_MAP);
            AmbitionApp.RegisterLink<CheckEndTutorialLink>(TutorialConsts.TUTORIAL_MACHINE, "ShowTutorialView", "EndTutorialParty");

            AmbitionApp.InvokeMachine(TutorialConsts.TUTORIAL_MACHINE);
            //AmbitionApp.InvokeMachine("TutorialConversationController");
        }
    }
}
