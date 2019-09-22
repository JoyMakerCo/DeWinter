using System;
using UFlow;
namespace Ambition
{
    public class OptionInputLink : ULink
    {
        public override bool Validate() => false;
        public override void Initialize() => AmbitionApp.Subscribe<TransitionVO>(HandleTransition);
        public override void Dispose() => AmbitionApp.Unsubscribe<TransitionVO>(HandleTransition);
        private void HandleTransition(TransitionVO transition)
        {
            if (transition != null)
            {
                IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
                TransitionVO[] trans = model.Incident?.GetLinks(model.Moment);
                if (trans == null || trans.Length == 0) return;
                int index = Array.IndexOf(trans, transition);
                model.Moment = model.Incident?.GetNeighbors(model.Moment)[index];
            }
        }
    }
}
