using System;
using Core;

namespace Ambition
{
    public class StartIncidentCmd : ICommand<IncidentVO>
    {
        public void Execute(IncidentVO incident)
        {
            AmbitionApp.InvokeMachine("IncidentController");
        }
    }
}
