using UFlow;

namespace Ambition
{
    public class CheckIncidentLink : ULink
    {
        public override bool Validate()
        {
            return AmbitionApp.GetModel<CalendarModel>().Incident != null;
        }
    }
}
