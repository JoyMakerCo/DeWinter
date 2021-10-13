using System;
using System.Collections.Generic;
namespace Ambition
{
    public class PoliticalIncidentCmd : Core.ICommand<CalendarModel>
    {
        public void Execute(CalendarModel calendar)
        {
            IncidentModel story = AmbitionApp.Story;
            IncidentVO incident = story.Incident;
            if (incident == null && Util.RNG.Generate(100) < AmbitionApp.Game.PoliticalChance)
            {
                string[] incidentIDs = story.Types[IncidentType.Political];
                List<IncidentVO> incidents = new List<IncidentVO>();
                foreach (string incidentID in incidentIDs)
                {
                    incident = story.GetIncident(incidentID);
                    if (AmbitionApp.CheckIncidentEligible(incident) && !incident.IsScheduled)
                    {
                        incidents.Add(incident);
                    }
                }
                if (incidents.Count > 0)
                {
                    incident = Util.RNG.TakeRandom(incidents);
                    AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, incident);
                }
            }
        }
    }
}
