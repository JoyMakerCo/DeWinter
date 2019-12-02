using Core;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class ChooseLocationCmd : ICommand<LocationVO>
    {
        public void Execute(LocationVO location)
        {
            if (location == null) return;
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            model.Location = location;
            AmbitionApp.GetModel<LocalizationModel>().SetLocation(location.LocationID);
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();

            if (!model.Visited.Contains(location.LocationID))
            {
                calendar.Schedule(location.IntroIncident, calendar.Today);
                model.Visited.Add(location.LocationID);
            }
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
                    int playCount = AmbitionApp.GetModel<IncidentModel>().GetPlayCount(ivo.Name);
                    Debug.Log("Play Count: "+playCount.ToString()); 
                    if (ivo.OneShot && (playCount > 0))
                    {
                        continue;
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
        }
    }
}
