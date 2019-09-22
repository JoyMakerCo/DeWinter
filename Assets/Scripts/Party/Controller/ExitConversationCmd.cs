using Core;
namespace Ambition
{
    public class ExitConversationCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.UnregisterModel<ConversationModel>();
        }
    }
}
