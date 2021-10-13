using System;
namespace Ambition
{
    public class RestAtHomeCmd : Core.ICommand
    {
        public void Execute()
        {
            AmbitionApp.Game.Exhaustion = (AmbitionApp.Game.Exhaustion > 0 ? 0 : -1);
        }
    }
}
