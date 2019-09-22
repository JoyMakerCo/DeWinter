using System;
namespace Ambition
{
    public class CheckEndIncidentLink : UFlow.ULink
    {
        public override bool Validate() => false;
        public override void Initialize() => AmbitionApp.Subscribe<IncidentVO>(IncidentMessages.END_INCIDENT, HandleEndIncident);
        public override void Dispose() => AmbitionApp.Unsubscribe<IncidentVO>(IncidentMessages.END_INCIDENT, HandleEndIncident);
        private void HandleEndIncident(IncidentVO incident) => Activate();
    }
}
