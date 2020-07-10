using System;
namespace Ambition
{
    public class CheckMomentDecision : UFlow.ULink
    {
        public override bool Validate() => AmbitionApp.GetModel<IncidentModel>().Moment != null;
    }
}
