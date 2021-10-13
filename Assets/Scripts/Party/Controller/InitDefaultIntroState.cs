using UFlow;
using UnityEngine;
using System;
using System.Collections.Generic;
namespace Ambition
{
    public class InitDefaultIntroState : UState
    {
        public override void OnEnter()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            IncidentConfig[] configs = Resources.LoadAll<IncidentConfig>(Filepath.INCIDENTS_PARTY_INTRO);
            List<IncidentVO> results = new List<IncidentVO>();
            IncidentVO incident;
            FactionType faction = model.Party.Faction;
            bool foundFaction = false;
            bool found;
            foreach (IncidentConfig config in configs)
            {
                incident = config?.GetIncident();
                if (incident != null)
                {
                    if (!foundFaction)
                    {
                        if (incident.Factions == null || incident.Factions.Length == 0)
                        {
                            results.Add(incident);
                        }
                        else
                        {
                            found = Array.IndexOf(incident.Factions, faction) >= 0;
                            if (found && faction == FactionType.None) results.Add(incident);
                            else
                            {
                                foundFaction = true;
                                results.Clear();
                                results.Add(incident);
                            }
                        }
                    }
                    else if (incident.Factions != null && Array.IndexOf(incident.Factions, faction) >= 0)
                    {
                        results.Add(incident);
                    }
                }
            }
            incident = Util.RNG.TakeRandom(results);
            AmbitionApp.Story.Schedule(incident);
        }
    }
}
