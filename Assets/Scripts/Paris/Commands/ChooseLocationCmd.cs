using Core;
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
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();

            if (!model.Visited.Contains(location.ID))
            {
                if (location.IntroIncident != null)
                {
                    calendar.Schedule(location.IntroIncident, calendar.Today);
                }
                foreach(IncidentVO incident in location.StoryIncidents)
                {
                    if (incident != null)
                    {
                        calendar.Schedule(incident);
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
                    foreach (var line in ivo.Dump())
                    {
                        Debug.Log("   "+line);
                    }
                    if (AmbitionApp.GetModel<IncidentModel>().PlayCount.TryGetValue(ivo.Name, out int count))
                    {
                        Debug.Log("Play Count for " + ivo.Name + ": " + count.ToString());
                        if (count > 0) continue;
                    }

                    if (AmbitionApp.CheckRequirements(ivo.Requirements))
                    {
                        Debug.LogFormat("-  "+ivo.ToString()+ " met requirements");

                        ivo.IsComplete = false; // dunno if this is necessary or a good idea...
                        calendar.Schedule(ivo, calendar.Today);
                        break;
                    }
                    else
                    {
                        Debug.LogFormat("-  "+ivo.ToString()+ " did not meet requirements");

                    }
                }
            }
#endif
        }
    }
}
