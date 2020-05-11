using System;
namespace Ambition
{
    public class TransitionInputCmd : Core.ICommand<TransitionVO>
    {
        public void Execute(TransitionVO transition)
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            if (transition?.Rewards?.Length > 0)
            {
                AmbitionApp.SendMessage(transition.Rewards);
            }
            model.Moment = model.Incident?.GetNextNode(transition);
            AmbitionApp.SendMessage(IncidentMessages.TRANSITION);
        }
    }
}
