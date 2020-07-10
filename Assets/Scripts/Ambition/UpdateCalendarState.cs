using System;
using System.Collections.Generic;
using UFlow;
using Util;
namespace Ambition
{
    public class UpdateCalendarState : UState
    {
        public override void OnEnterState()
        {
            GameModel model = AmbitionApp.GetModel<GameModel>();
            AmbitionApp.GetModel<LocalizationModel>().SetDate(model.Date);
            PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
            partyModel.SetDay(model.Day);
            PartyVO party = partyModel.UpdateParty();
            PartyVO[] parties;
            IncidentModel incidentModel = AmbitionApp.GetModel<IncidentModel>();
            incidentModel.SetDay(model.Day);

            for (int i = 0; i < 14; i++)
            {
                parties = partyModel.GetParties((ushort)(model.Day + i));
                foreach (PartyVO vO in parties)
                {
                    if (vO.RSVP == RSVP.New)
                        AmbitionApp.SendMessage(PartyMessages.INITIALIZE_PARTY, party);
                }
            }

            // If there's no morning incident scheduled, then there's a chance to select one randomly.
            if (incidentModel.Incident == null && RNG.Generate(100) < model.PoliticalChance)
            {
                IncidentVO[] incidents = incidentModel.GetIncidents(IncidentType.Timeline);
                List<IncidentVO> candidates = new List<IncidentVO>();
                foreach (IncidentVO incident in incidents)
                {
                    if (AmbitionApp.CheckRequirements(incident.Requirements))
                    {
                        candidates.Add(incident);
                    }
                    incidentModel.Schedule(RNG.TakeRandom(candidates));
                }
            }
        }
    }
}
