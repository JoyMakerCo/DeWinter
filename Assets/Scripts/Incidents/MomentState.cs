using System;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
    public class MomentState : UState
    {
        public override void OnEnter()
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            MomentVO moment = model.UpdateMoment();
            AmbitionApp.GetModel<LocalizationModel>().SetMoment(moment);
            AmbitionApp.SendMessage(moment);
            AmbitionApp.SendMessage(moment.Rewards);

            TransitionVO[] links = model.Incident.GetLinks(moment);
            AmbitionApp.SendMessage(links);
        }
    }
}
