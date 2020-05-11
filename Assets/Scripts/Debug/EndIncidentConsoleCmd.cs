#if DEBUG
using System;
namespace Ambition
{
    public class EndIncidentConsoleCmd : Core.ICommand
    {
        public void Execute()
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            AmbitionApp.GetModel<CalendarModel>().Complete(model.Incident);
        }
    }
}
#endif