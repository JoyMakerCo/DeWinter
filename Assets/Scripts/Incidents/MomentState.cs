using System;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
    public class MomentState : UState
    {
        public override void OnEnterState()
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            MomentVO moment = model.Moment;
            AmbitionApp.SendMessage(moment);
            AmbitionApp.SendMessage(moment.Rewards);
            AmbitionApp.GetModel<LocalizationModel>().SetMoment(moment);

            List<TransitionVO> result = new List<TransitionVO>(); 
            TransitionVO[] links = model.Incident.GetLinks(moment);
            bool xor = false;
            foreach(TransitionVO trans in links)
            {
                if (AmbitionApp.CheckRequirements(trans.Requirements)
                    && !(trans.xor && xor))
                {
                    trans.index = Array.IndexOf(model.Incident.LinkData, trans);
                    result.Add(trans);
                    if (trans.xor) xor = true;
                }
            }
            AmbitionApp.SendMessage(result.ToArray());
        }
    }
}
