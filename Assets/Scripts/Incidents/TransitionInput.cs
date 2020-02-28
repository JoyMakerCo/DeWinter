using System;
using UFlow;

namespace Ambition
{
    public class TransitionInput : UInputState
    {
        public override void Initialize(object[] parameters) => AmbitionApp.Subscribe<TransitionVO>(HandleTransition);
        public override void Dispose() => AmbitionApp.Unsubscribe<TransitionVO>(HandleTransition);

        private void HandleTransition(TransitionVO transition)
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            model.Moment = model.Incident?.GetNextNode(transition);
            if (model.Moment != null) Activate();
        }
    }
}
