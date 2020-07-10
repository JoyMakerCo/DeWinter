using System;
namespace Ambition
{
    public class PickLocationState : UFlow.UState
    {
        public override void OnEnterState()
        {
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();
            AmbitionApp.GetModel<LocalizationModel>().SetLocation(paris.Location?.ID);
            if (!string.IsNullOrEmpty(paris.Location?.IntroIncident))
            {
                IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
                if (model.Incidents.TryGetValue(paris.Location.IntroIncident, out IncidentVO incident))
                {
                    AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, incident);
                }
            }
        }
    }
}
