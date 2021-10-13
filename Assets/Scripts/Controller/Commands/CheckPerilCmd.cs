using System;
namespace Ambition
{
    public class CheckPerilCmd : Core.ICommand<int>
    {
        public void Execute(int peril)
        {
            if (peril >= 100)
                AmbitionApp.Game.Perilous = true;
        }
    }
}
