using System;
namespace Ambition
{
    public class PickLocationState : UFlow.UState
    {
        public override void OnEnterState()
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            AmbitionApp.GetModel<LocalizationModel>().SetLocation(model.Location?.ID);
            AmbitionApp.GetModel<CalendarModel>().Schedule(model.Location?.IntroIncident);
        }
    }
}
