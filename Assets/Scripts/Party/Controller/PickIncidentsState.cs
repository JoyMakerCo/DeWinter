using System;
using System.Collections.Generic;
using System.Linq;
using UFlow;
using Util;
namespace Ambition
{
    public class PickIncidentsState : UState
    {
        private PartyModel _model;
        public override void OnEnterState()
        {
            _model = AmbitionApp.GetModel<PartyModel>();
            List<IncidentVO> result = new List<IncidentVO>();
            if (_model.RequiredIncident != null) result.Add(_model.RequiredIncident);
            if (_model.RequiredIncident == null || _model.TurnsLeft > (_model.Party.RequiredIncidents.Length - _model.IncidentIndex))
            {
                int count = (int)(_model.Party.Size);
                IEnumerable<IncidentVO> added = TakeRandom(_model.Party.SupplementalIncidents, result.Count, count - result.Count);
                result.AddRange(added.Where(i=>!i.IsComplete));

                // TODO: NEED RANDOM PARTIES
                if (false)
                //if (result.Count < count)
                {
                    added = (from i in AmbitionApp.GetModel<CalendarModel>().GetUnscheduledEvents<IncidentVO>()
                              where !i.IsComplete
                                 && (Array.IndexOf(i.Factions, _model.Party.Faction) >= 0 || Array.IndexOf(i.Factions, FactionType.Neutral) >= 0)
                                 && (i.Chapters == null || i.Chapters.Length == 0 || Array.IndexOf(i.Chapters, AmbitionApp.GetModel<GameModel>().Chapter) >= 0)
                                 && AmbitionApp.CheckRequirements(i.Requirements)
                              select i);
                    IEnumerable<IncidentVO> characterIncidents = added.Where(i => i.GetCharacters().Intersect(_model.Party.Guests.Select(g => g.Name)).Count() >= 1);
                    added = TakeRandom(characterIncidents, result.Count, count - result.Count);
                    result.AddRange(added);
                    if (added.Count() < count)
                    {
                        added = added.Except(characterIncidents);
                        added = TakeRandom(added, result.Count, count - result.Count);
                        result.AddRange(added);
                    }
                }
            }
            _model.Incidents = result.ToArray();
            AmbitionApp.SendMessage(PartyMessages.SELECT_INCIDENTS, _model.Incidents);
        }

        private IEnumerable<IncidentVO> TakeRandom(IEnumerable<IncidentVO> incidents, int from, int count)
        {
            List<IncidentVO> result = new List<IncidentVO>();
            if (incidents == null) return result;
            IncidentVO[] arr = incidents.ToArray();
            int numIncidents = arr.Length;
            if (numIncidents - from < count) count = numIncidents - from;
            int index;
            IncidentVO temp;
            for (int i = 0; i < numIncidents && result.Count < count; i++)
            {
                index = RNG.Generate(i, numIncidents);
                temp = arr[index];
                arr[index] = arr[i];
                arr[i] = temp;
                if (temp != null && !temp.IsComplete)
                    result.Add(temp);
            }
            return result;
        }
    }
}