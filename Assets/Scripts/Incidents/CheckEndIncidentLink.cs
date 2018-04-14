using UFlow;

namespace Ambition
{
    public class CheckEndIncidentLink : ULink
    {
        override public bool InitializeAndValidate()
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            return model.Moment == null;
        }
    }
}
