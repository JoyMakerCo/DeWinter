using System;
using System.Collections.Generic;
using UFlow;
using Util;
namespace Ambition
{
    public class PickIncidentsState : UState, Core.IState
    {
        public override void OnEnter()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            IncidentVO[] incidents = new IncidentVO[model.NumRooms];
            IncidentModel story = AmbitionApp.Story;
            if (model.Incidents != null)
            {
                incidents = new IncidentVO[model.NumRooms];
                for (int i = model.NumRooms - 1; i >= 0; --i)
                {
                    incidents[i] = story.GetIncident(model.Incidents[i]);
                }
            }
            else
            {
                HashSet<string> characters = new HashSet<string>();
                if (model.RequiredIncident != null)
                {
                    int index = RNG.Generate(model.NumRooms);
                    incidents[index] = story.GetIncident(model.RequiredIncident);
                }
                else
                {
                    List<IncidentVO> result = new List<IncidentVO>();
                    IncidentVO incident;
                    if (model.Party.SupplementalIncidents != null)
                    {
                        string[] shuffle = RNG.Shuffle(model.Party.SupplementalIncidents);
                        foreach (string incidentID in shuffle)
                        {
                            incident = story.GetIncident(incidentID);
                            if (result.Count < model.NumRooms)
                            {
                                if (FindCharacters(incident, characters))
                                {
                                    result.Add(incident);
                                }
                            }
                        }
                    }

                    if (result.Count < model.NumRooms)
                    {
                        incidents = story.GetIncidents(IncidentType.Party);
                        incidents = RNG.Shuffle(incidents);
                        foreach (IncidentVO rand in incidents)
                        {
                            if (AmbitionApp.CheckIncidentEligible(rand)
                                && (rand.Factions?.Length == 0 || Array.IndexOf(rand.Factions, model.Party.Faction) >= 0)
                                && FindCharacters(rand, characters))
                            {
                                result.Add(rand);
                            }
                            if (result.Count >= model.NumRooms)
                                break;
                        }
                    }
                    while (result.Count < model.NumRooms)
                    {
                        result.Add(null);
                    }
                    incidents = RNG.Shuffle(result);
                }
                model.Incidents = new string[model.NumRooms];
                for (int i = model.NumRooms - 1; i >= 0; --i)
                {
                    model.Incidents[i] = incidents[i]?.ID;
                }
            }

            AmbitionApp.SendMessage(PartyMessages.SELECT_INCIDENTS, incidents);
            string title = AmbitionApp.Localize(PartyConstants.PARTY_NAME + model.Party.ID);
            if (string.IsNullOrEmpty(title)) title = PartyConstants.PARTY_REASON + model.Party.Faction.ToString().ToLower() + "." + model.Party.phrases[0];
            AmbitionApp.SendMessage(GameMessages.SHOW_HEADER, title);
        }

        private bool FindCharacters(IncidentVO incident, HashSet<string> characters)
        {
            if (incident?.Nodes == null) return false;
            HashSet<string> local = new HashSet<string>();
            foreach(MomentVO moment in incident.Nodes)
            {
                if (!string.IsNullOrEmpty(moment.Character1.Name) && characters.Contains(moment.Character1.Name))
                    return false;
                local.Add(moment.Character1.Name);
                if (!string.IsNullOrEmpty(moment.Character2.Name) && characters.Contains(moment.Character2.Name))
                    return false;
                local.Add(moment.Character2.Name);
            }
            characters.UnionWith(local);
            return true;
        }
    }
}
