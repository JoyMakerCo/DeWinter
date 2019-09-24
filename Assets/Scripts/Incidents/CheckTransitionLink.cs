using System;
using UFlow;

namespace Ambition
{
    public class CheckTransitionLink : ULink
    {
        public override bool Validate() => false;
        public override void Initialize() => AmbitionApp.Subscribe<TransitionVO>(HandleTransition);
        public override void Dispose() => AmbitionApp.Unsubscribe<TransitionVO>(HandleTransition);
        private void HandleTransition(TransitionVO transition)
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            TransitionVO[] trans = model.Incident?.GetLinks(model.Moment);
            if (trans == null || trans.Length == 0)
            {
                AmbitionApp.SendMessage(IncidentMessages.END_INCIDENT, model.Incident);
            }
            else
            {
                // fire rewards on transition
                AmbitionApp.SendMessage(transition.Rewards);
                model.Moment = model.Incident?.GetNextNode(transition);
                Activate();
            }
        }
    }
}
