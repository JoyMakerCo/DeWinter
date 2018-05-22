using UFlow;

namespace Ambition
{
    public class CheckIncidentLink : ULink
    {
        override public void Initialize()
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
           if (model.Incident != null) Activate();
        }
    }
}
