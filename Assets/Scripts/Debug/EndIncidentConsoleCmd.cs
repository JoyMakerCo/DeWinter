#if DEBUG
using System;
namespace Ambition
{
    public class EndIncidentConsoleCmd : Core.ICommand
    {
        public void Execute()
        {
            AmbitionApp.GetModel<IncidentModel>().NextIncident();
        }
    }
}
#endif