using UFlow;

namespace Ambition
{
    public class CheckIncidentLink : ULink
    {
        override public bool InitializeAndValidate()
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            return model.Incident != null;
        }
    }
}
