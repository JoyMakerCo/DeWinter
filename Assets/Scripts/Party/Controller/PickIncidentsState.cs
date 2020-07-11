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
            int i, index;

            if (pmodel.RequiredIncident != null)
            {
                index = RNG.Generate(pmodel.NumRooms);
                for (i = pmodel.NumRooms - 1; i >= 0; --i)
                {
                    pmodel.Incidents[i] = (i == index)
                        ? pmodel.RequiredIncident
                        : null;
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
                    IncidentVO[] random = imodel.GetIncidents(IncidentType.Party);
                    foreach(IncidentVO rand in random)
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
            AmbitionApp.SendMessage(PartyMessages.SELECT_INCIDENTS, pmodel.Incidents);
        }
    }
}