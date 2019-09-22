using System;
using Core;
namespace Ambition
{
    public class ShowRoomCmd : ICommand<IncidentVO>
    {
        public void Execute(IncidentVO incident)
        {
            if (incident != null)
            {
                PartyModel model = AmbitionApp.GetModel<PartyModel>();
                model.Incident = incident;
                AmbitionApp.GetModel<CalendarModel>().Schedule(model.Incident);
                AmbitionApp.SendMessage(PartyMessages.SHOW_ROOM);
            }
        }
    }
}
