using UFlow;

namespace Ambition
{
    public class CheckEndIncidentLink : ULink
    {
        override public void Initialize()
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            if (model.Moment == null) Activate();
        }
    }
}
