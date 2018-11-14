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
            AmbitionApp.RegisterState<TutorialRemarkState>(TutorialConsts.TUTORIAL_MACHINE, "TutorialRemarkStep");
            AmbitionApp.RegisterState<TutorialGuestState>(TutorialConsts.TUTORIAL_MACHINE, "TutorialGuestStep");
            AmbitionApp.RegisterState<TutorialGuestState>(TutorialConsts.TUTORIAL_MACHINE, "EndConversationTutorial");
            AmbitionApp.RegisterState<EndTutorialState>(TutorialConsts.TUTORIAL_MACHINE, "EndTutorialParty");

            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>(TutorialConsts.TUTORIAL_MACHINE, "TutorialStart", "ShowTutorialView", PartyMessages.SHOW_MAP);
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>(TutorialConsts.TUTORIAL_MACHINE, "ShowTutorialView", "TutorialRemarkStep", PartyMessages.START_ROUND);
            AmbitionApp.RegisterLink<TutorialRemarkLink>(TutorialConsts.TUTORIAL_MACHINE, "TutorialRemarkStep", "TutorialGuestStep");
            AmbitionApp.RegisterLink<TutorialRemarkLink>(TutorialConsts.TUTORIAL_MACHINE, "TutorialGuestStep", "TutorialGuestStep");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>(TutorialConsts.TUTORIAL_MACHINE, "TutorialGuestStep", "EndConversationTutorial", PartyMessages.END_ROUND);
            AmbitionApp.RegisterLink<CheckEndTutorialLink>(TutorialConsts.TUTORIAL_MACHINE, "EndConversationTutorial", "EndTutorialParty");

            AmbitionApp.InvokeMachine(TutorialConsts.TUTORIAL_MACHINE);
            //AmbitionApp.InvokeMachine("TutorialConversationController");
        }
    }
}
