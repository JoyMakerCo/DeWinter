using Core;

namespace Ambition
{
    public class ChooseLocationCmd : ICommand<LocationPin>
    {
        public void Execute(LocationPin location)
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            model.Location = location;
            if (location != null)
            {
                if (location.Name() == "Home") model.Location = null;
                else {
                    IncidentVO incident = location.IntroIncidentConfig?.Incident;
                    if (location != null && incident != null && (!location.Visited || !incident.OneShot))
                    {
                        CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                        calendar.Incident = location.IntroIncidentConfig.Incident;
                    }
                    location.Visited = true;
                }
            }
        }
    }
}
