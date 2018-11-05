using Core;
namespace Ambition
{
    public class DrawOneCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.SendMessage(PartyMessages.DRAW_REMARKS, 1);
        }
    }
}
