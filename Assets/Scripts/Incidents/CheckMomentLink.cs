using UFlow;

namespace Ambition
{
    public class CheckEndIncidentLink : ULink
    {
        public override bool Validate()
        {
            CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
            if (model.Moment == null)
            {
                model.Incident = null;
                model.Moment = null;
                return true;
            }
            return false;
        }
    }
}
