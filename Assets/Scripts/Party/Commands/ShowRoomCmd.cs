using System;
using Core;
namespace Ambition
{
    public class ShowRoomCmd : ICommand<string>
    {
        public void Execute(string incidentID)
        {
            AmbitionApp.GetModel<IncidentModel>().Schedule(incidentID);
            AmbitionApp.SendMessage(PartyMessages.SHOW_ROOM);
        }
    }
}
