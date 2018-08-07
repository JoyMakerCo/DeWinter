using UFlow;

namespace Ambition
{
    public class CheckEndIncidentLink : ULink
    {
        override public void Initialize()
        {
            CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
            if (model.Moment == null) Activate();
        }
    }
}
