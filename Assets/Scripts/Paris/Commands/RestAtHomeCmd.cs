using System;
using Core;
using UFlow;

namespace Ambition
{
    public class RestAtHomeCmd : ICommand
    {
        public void Execute() => AmbitionApp.GetModel<GameModel>().IsResting = true;
    }
}
