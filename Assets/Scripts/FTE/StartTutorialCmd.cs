using Core;

namespace Ambition
{
    public class StartTutorialCmd : ICommand
    {
        public void Execute()
        {
			AmbitionApp.RegisterCommand<TutorialConfidenceCheckCmd>(PartyMessages.SHOW_MAP);

			AmbitionApp.UnregisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			AmbitionApp.UnregisterCommand<SkipTutorialCmd>(GameMessages.SKIP_TUTORIAL);
            AmbitionApp.InvokeMachine("TutorialController");
        }
    }
}
