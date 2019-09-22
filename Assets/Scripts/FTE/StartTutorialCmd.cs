using Core;

namespace Ambition
{
    public class StartTutorialCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.UnregisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
            AmbitionApp.UnregisterCommand<SkipTutorialCmd>(GameMessages.SKIP_TUTORIAL);

            AmbitionApp.RegisterCommand<TutorialFleeConversationCmd>(PartyMessages.FLEE_CONVERSATION);
            AmbitionApp.RegisterCommand<DisableTimerCmd>(TutorialMessage.DISABLE_TIMER);

            AmbitionApp.RegisterState(TutorialConsts.TUTORIAL_MACHINE, "TutorialStart");
            AmbitionApp.RegisterState<TutorialState>(TutorialConsts.TUTORIAL_MACHINE, "ShowTutorialView");
            AmbitionApp.RegisterState<SendMessageState, string>(TutorialConsts.TUTORIAL_MACHINE, "TutorialDisableClock", TutorialMessage.DISABLE_TIMER);

            AmbitionApp.RegisterState<TutorialRemarkState>(TutorialConsts.TUTORIAL_MACHINE, "TutorialRemarkStep");
            AmbitionApp.RegisterState<TutorialGuestState>(TutorialConsts.TUTORIAL_MACHINE, "TutorialGuestStep");
            AmbitionApp.RegisterState<TutorialGuestState>(TutorialConsts.TUTORIAL_MACHINE, "EndConversationTutorial");
            AmbitionApp.RegisterState<EndTutorialState>(TutorialConsts.TUTORIAL_MACHINE, "EndTutorialParty");

            AmbitionApp.RegisterLink<MessageLink, string>(TutorialConsts.TUTORIAL_MACHINE, "TutorialStart", "ShowTutorialView", PartyMessages.SHOW_MAP);
            AmbitionApp.RegisterLink<MessageLink, string>(TutorialConsts.TUTORIAL_MACHINE, "ShowTutorialView", "TutorialDisableClock", PartyMessages.SHOW_ROOM);
            AmbitionApp.RegisterLink<MessageLink, string>(TutorialConsts.TUTORIAL_MACHINE, "TutorialDisableClock", "TutorialRemarkStep", PartyMessages.START_ROUND);
            AmbitionApp.RegisterLink<TutorialRemarkLink>(TutorialConsts.TUTORIAL_MACHINE, "TutorialRemarkStep", "TutorialGuestStep");
            AmbitionApp.RegisterLink<TutorialRemarkLink>(TutorialConsts.TUTORIAL_MACHINE, "TutorialGuestStep", "TutorialGuestStep");
            AmbitionApp.RegisterLink<MessageLink, string>(TutorialConsts.TUTORIAL_MACHINE, "TutorialGuestStep", "EndConversationTutorial", PartyMessages.END_ROUND);
            AmbitionApp.RegisterLink<CheckEndTutorialLink>(TutorialConsts.TUTORIAL_MACHINE, "EndConversationTutorial", "EndTutorialParty");

            App.Service<UFlow.UFlowSvc>().InvokeMachine(TutorialConsts.TUTORIAL_MACHINE);
        }
    }
}
