using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
namespace Ambition
{
    public class LoadPartyCmd : ICommand<string>
    {
        public void Execute(string file)
        {
            PartyConfig config = Resources.Load<PartyConfig>(Filepath.PARTIES + file);
            PartyVO party = AmbitionApp.GetModel<PartyModel>().LoadConfig(config);
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();

            AmbitionApp.SendMessage(PartyMessages.INITIALIZE_PARTY, party);
            AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, party);

            model.AddDependency(config.IntroIncident, file, IncidentType.Party);
            model.AddDependency(config.ExitIncident, file, IncidentType.Party);
            Array.ForEach(config.SupplementalIncidents, i => model.AddDependency(i, file, IncidentType.Party));
            Array.ForEach(config.RequiredIncidents, i => model.AddDependency(i, file, IncidentType.Party));
        }
    }
}
