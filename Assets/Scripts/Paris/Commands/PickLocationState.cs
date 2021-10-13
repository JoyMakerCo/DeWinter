using System;
using System.Collections.Generic;
namespace Ambition
{
    public class PickLocationState : UFlow.UState
    {
        public override void OnEnter()
        {
            LocationVO location = AmbitionApp.Paris.GetLocation();
            IncidentVO incident = GetLocationIncident(location);
            AmbitionApp.Game.Activity = ActivityType.Paris;
            if (incident != null)
            {
                incident.Date = AmbitionApp.Calendar.Today;
                AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, incident);
            }
        }

        private IncidentVO GetLocationIncident(LocationVO location)
        {
            ParisModel paris = AmbitionApp.Paris;
            if (location == null || paris.IsComplete(location.ID)) return null;

            AmbitionApp.GetModel<LocalizationModel>().SetLocation(location.ID);
            if (AmbitionApp.CheckIncidentEligible(location.IntroIncident))
                return AmbitionApp.Story.GetIncident(location.IntroIncident);
            foreach (string incidentID in location.StoryIncidents)
            {
                if (AmbitionApp.CheckIncidentEligible(incidentID))
                    return AmbitionApp.Story.GetIncident(incidentID);
            }
            return null;
        }
    }
}
