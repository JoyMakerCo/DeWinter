using Core;
namespace Ambition
{
    public class SkipTutorialCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.UnregisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
            AmbitionApp.UnregisterCommand<SkipTutorialCmd>(GameMessages.SKIP_TUTORIAL);
            AmbitionApp.RegisterCommand<FleeConversationCmd>(PartyMessages.FLEE_CONVERSATION);
        }
    }
}
