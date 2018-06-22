using UFlow;

namespace Ambition
{
    public class CheckIncidentLink : AmbitionValueLink<IncidentVO>
    {
        public CheckIncidentLink() { ValidateOnInit = true; }
        override protected bool Validate(IncidentVO incident)
        {
            return AmbitionApp.GetModel<IncidentModel>().Incident != null;
        }
    }
}
