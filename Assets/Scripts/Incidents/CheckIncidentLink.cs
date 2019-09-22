using UFlow;

namespace Ambition
{
    public class CheckIncidentLink : ULink
    {
        public override bool Validate() => AmbitionApp.GetModel<IncidentModel>().Incident != null;
    }
}
