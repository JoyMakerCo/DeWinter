using Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class ChooseLocationCmd : ICommand<LocationVO>
    {
        public void Execute(LocationVO location)
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            model.Location = location;
            if (location == null || (location.OneShot && model.Visited.Contains(location.ID)))
            {
                return;
            }

            AmbitionApp.GetModel<LocalizationModel>().SetLocation(location?.ID);

            if (!model.Visited.Contains(location.ID))
            {
                IncidentModel incidentModel = AmbitionApp.GetModel<IncidentModel>();
                CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                incidentModel.Schedule(location.IntroIncident);
                foreach(string incidentID in location.StoryIncidents)
                {
                    if (incidentModel.Incidents.TryGetValue(incidentID, out IncidentVO incident))
                    {
                        if (incident.IsScheduled)
                        {
                            incidentModel.Schedule(incidentID, incident.Date.Subtract(calendar.StartDate).Days);
                        }
                        else incidentModel.Schedule(incidentID);
                    }
                }
                model.Visited.Add(location.ID);
            }
#if DEBUG
            else
            {
                Debug.Log("Checking story incidents");
                // iterate over story incidents until we find one with met requirements
                foreach (var ivo in location.StoryIncidents)
                {
                    Debug.Log(" checking: ");
                    foreach (var line in ivo)
                    {
                        Debug.Log("   "+line);
                    }
                }
            }
#endif
        }
    }
}
