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

            TransitionVO[] links = model.Incident.GetLinks(moment);
            TransitionVO xor = null;
            for (int i = 0; i < links.Length; i++)
            {
                if (!AmbitionApp.CheckRequirements(links[i]?.Requirements))
                {
                    links[i] = null;
                }
                else
                {
                    links[i].index = Array.IndexOf(model.Incident.LinkData, links[i]);
                    if (links[i].xor)
                    {
                        if (xor == null)
                        {
                            xor = links[i];
                        }
                        else
                        {
                            links[i] = null;
                        }
                    }
                }
            }
            AmbitionApp.SendMessage(links);
        }
    }
}
