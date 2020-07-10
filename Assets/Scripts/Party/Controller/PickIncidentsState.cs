using System;
using System.Collections.Generic;
using UFlow;
using Util;
namespace Ambition
{
    public class PickIncidentsState : UState
    {
        private List<string> _guests;

        public override void OnEnterState()
        {
            PartyModel pmodel = AmbitionApp.GetModel<PartyModel>();
            IncidentModel imodel = AmbitionApp.GetModel<IncidentModel>();
            IncidentVO incident;
            IncidentVO[] result;
            int i, index;
            string incidentID = pmodel.GetRequiredIncident();

            if (!string.IsNullOrEmpty(incidentID))
            {
                result = new IncidentVO[pmodel.NumRooms];
                index = RNG.Generate(pmodel.NumRooms);
                for (i = pmodel.NumRooms - 1; i >= 0; --i)
                {
                    if (i == index)
                    {
                        pmodel.Incidents[i] = incidentID;
                        imodel.Incidents.TryGetValue(incidentID, out incident);
                        result[i] = incident;
                    }
                    else
                    {
                        pmodel.Incidents[i] = null;
                        result[i] = null;
                    }
                }
            }
            else
            { 
                List<string> incidents = new List<string>(pmodel.Party.SupplementalIncidents);
                int[] shuffle = new int[pmodel.NumRooms];
                int temp = pmodel.NumRooms - incidents.Count;

                if (temp > 0)
                {
                    List<string> names = new List<string>();
                    result = imodel.GetIncidents(IncidentType.Party);
                    foreach(IncidentVO rand in result)
                    {
                        if (!imodel.IsComplete(rand.ID) && AmbitionApp.CheckRequirements(rand.Requirements))
                        {
                            names.Add(rand.ID);
                        }
                    }
                    if (names.Count <= temp) incidents.AddRange(names);
                    else while (temp > 0)
                    {
                        index = RNG.Generate(names.Count);
                        incidents.Add(names[index]);
                        names.RemoveAt(index);
                        --temp;
                    }
                }

                for (i = shuffle.Length - 1; i >= 0; --i) shuffle[i] = i;
                for (i= shuffle.Length - 1; i >= 0; --i)
                {
                    temp = shuffle[i];
                    index = RNG.Generate(i);
                    shuffle[i] = shuffle[index];
                    shuffle[index] = temp;
                    pmodel.Incidents[i] = shuffle[i] < incidents.Count ? incidents[shuffle[i]] : null;
                }
            }

            result = new IncidentVO[pmodel.Incidents.Length];
            for (i=result.Length-1; i>=0; --i)
            {
                if (pmodel.Incidents[i] != null)
                {
                    imodel.Incidents.TryGetValue(pmodel.Incidents[i], out incident);
                    if (incident == null) pmodel.Incidents[i] = null;
                    result[i] = incident;
                }
                else
                {
                    result[i] = null;
                }
            }
            AmbitionApp.SendMessage(PartyMessages.SELECT_INCIDENTS, result);
        }
    }
}