using Core;
using System.Collections.Generic;

namespace Ambition
{
    public class ChooseLocationCmd : ICommand<Pin>
    {
        public void Execute(Pin location)
        {
            if (location == null) return;
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            model.Location = location;
            if (!model.Visited.Contains(location.name))
            {
                CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                calendar.Schedule(location.IntroIncidentConfig?.GetIncident(), calendar.Today);
                model.Visited.Add(location.name);
            }
        }
    }
}
