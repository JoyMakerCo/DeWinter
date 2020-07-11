using System;
using Core;
namespace Ambition
{
    public class SelectIncidentsCmd : ICommand<string[]>
    {
        public void Execute(string [] incidentIDs)
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            IncidentVO[] result = new IncidentVO[incidentIDs.Length];
            for(int i=incidentIDs.Length-1; i>=0; --i)
            {
                result[i] = model.LoadIncident(incidentIDs[i]);
            }
            AmbitionApp.SendMessage(PartyMessages.SELECT_INCIDENTS, result);
        }
    }
}
