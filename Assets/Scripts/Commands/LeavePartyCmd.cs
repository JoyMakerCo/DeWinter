using Core;
namespace Ambition
{
    // DEPRECATED
    public class LeavePartyCmd : ICommand
    {
        public void Execute() { } //=> AmbitionApp.GetModel<PartyModel>().TurnsLeft = 0;
    }
}
