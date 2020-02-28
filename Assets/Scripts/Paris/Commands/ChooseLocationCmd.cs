﻿using Core;
using System.Collections.Generic;

namespace Ambition
{
    public class ChooseLocationCmd : ICommand<Pin>
    {
        public void Execute(Pin location)
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            model.Location = location;
<<<<<<< Updated upstream
            if (!model.Visited.Contains(location.name))
=======
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
>>>>>>> Stashed changes
            {
                CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                calendar.Schedule(location.IntroIncidentConfig?.GetIncident(), calendar.Today);
                model.Visited.Add(location.name);
            }
#endif
        }
    }
}
