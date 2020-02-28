using System;
using System.Collections.Generic;
using UFlow;
using Util;
namespace Ambition
{
    public class PickIncidentsState : UState
    {
        private PartyModel _model;
        private List<string> _guests;
        private List<IncidentVO> _incidents;

        public override void OnEnterState()
        {
            _model = AmbitionApp.GetModel<PartyModel>();
            if (_model.RequiredIncident != null)
            {
                AmbitionApp.SendMessage(PartyMessages.SELECT_INCIDENTS, new IncidentVO[] { _model.RequiredIncident });
                return; // Early out for required incident
            }

            int max = (int)(_model.Party.Size);
            int[] shuffle = new int[_model.Party.SupplementalIncidents?.Length ?? 0];
            IncidentVO incident;
            _incidents = new List<IncidentVO>();
            _guests = new List<string>();

            for (int i = shuffle.Length - 1; i > 0; --i)
            {
                shuffle[i] = RNG.Generate(i);
                shuffle[shuffle[i]] = i;
            }
            for (int i = shuffle.Length - 1; i >= 0; --i)
            {
                incident = _model.Party.SupplementalIncidents[shuffle[i]];
                if (!incident.IsComplete
                    && AddIncident(incident)
                    && _incidents.Count == max)
                {
                    AmbitionApp.SendMessage(PartyMessages.SELECT_INCIDENTS, _incidents.ToArray());
                    return; // Early out for supplemental incident
                }
            }

            IncidentVO[] unscheduled = AmbitionApp.GetModel<CalendarModel>().GetUnscheduledEvents<IncidentVO>();
            shuffle = new int[unscheduled.Length];
            for (int i = shuffle.Length - 1; i > 0; --i)
            {
                shuffle[i] = RNG.Generate(i);
                shuffle[shuffle[i]] = i;
            }
            for (int i = shuffle.Length - 1; i >= 0; --i)
            {
                incident = unscheduled[shuffle[i]];
                if (!incident.IsComplete
                    && CheckFaction(incident)
                    && AddIncident(incident)
                    && _incidents.Count == max)
                {
                    AmbitionApp.SendMessage(PartyMessages.SELECT_INCIDENTS, _incidents.ToArray());
                    return; // Early out for random incidents
                }
            }

            AmbitionApp.SendMessage(PartyMessages.SELECT_INCIDENTS, _incidents.ToArray());
        }

        private bool CheckFaction(IncidentVO incident)
        {
            if (incident == null) return false;
            foreach (FactionType faction in incident.Factions)
            {
                if (faction == _model.Party.Faction) return true;
            }
            return false;
        }

        private bool AddIncident(IncidentVO incident)
        {
            if (incident == null) return false;
            List<string> currentGuests = new List<string>();
            foreach(MomentVO moment in incident.Nodes)
            {
                if (!AddValidName(moment?.Character1.Name, currentGuests) || !AddValidName(moment?.Character2.Name, currentGuests))
                {
                    return false;
                }
            }
            _guests.AddRange(currentGuests);
            _incidents.Add(incident);
            return true;
        }

        private bool AddValidName(string guest, List<string> currentGuests)
        {
            if (!string.IsNullOrEmpty(guest)) return true;
            if (!currentGuests.Contains(guest))
            {
                if (_guests.Contains(guest)) return false;
                _guests.Add(guest);
                currentGuests.Add(guest);
            }
            return true;
        }
    }
}