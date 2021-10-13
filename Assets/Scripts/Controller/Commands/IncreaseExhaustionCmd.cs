using System;
namespace Ambition
{
    public class IncreaseExhaustionCmd : Core.ICommand
    {
        public void Execute() => ++AmbitionApp.Game.Exhaustion;
    }
}
