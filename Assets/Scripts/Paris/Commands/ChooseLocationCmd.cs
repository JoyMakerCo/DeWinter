using Core;
using System.Collections.Generic;

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
            if (!model.Visited.Contains(location.LocationID))
            {
                CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                calendar.Schedule(location.Incident, calendar.Today);
                model.Visited.Add(location.LocationID);
            }
        }
    }
}
