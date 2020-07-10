using System;
using System.Collections.Generic;
using Core;
using UFlow;
namespace Ambition
{
    public class FinishLoadGameCmd : ICommand
    {
        public void Execute()
        {
            IncidentModel incidentModel = AmbitionApp.GetModel<IncidentModel>();
            PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();

            if (incidentModel.Moment != null)
            {
                AmbitionApp.SendMessage(GameMessages.LOAD_SCENE, SceneConsts.INCIDENT_SCENE);
                AmbitionApp.SendMessage(GameMessages.SHOW_HEADER);
                AmbitionApp.SendMessage(IncidentMessages.START_INCIDENT, incidentModel.Incident);
                AmbitionApp.SendMessage(incidentModel.Moment);
                AmbitionApp.GetModel<LocalizationModel>().SetMoment(incidentModel.Moment);

                List<TransitionVO> transitions = new List<TransitionVO>();
                IncidentVO incident = incidentModel.Incident;
                TransitionVO[] links = incident.GetLinks(incidentModel.Moment);
                bool xor = false;
                foreach (TransitionVO trans in links)
                {
                    if (AmbitionApp.CheckRequirements(trans.Requirements)
                        && !(trans.xor && xor))
                    {
                        trans.index = Array.IndexOf(incident.LinkData, trans);
                        transitions.Add(trans);
                        if (trans.xor) xor = true;
                    }
                }
                AmbitionApp.SendMessage(transitions.ToArray());
            }
            else if (partyModel.Party != null && AmbitionApp.GetService<UFlowSvc>().IsActiveMachine(FlowConsts.PARTY_CONTROLLER))
            {
                if (partyModel.Turn < 0)
                {
                    AmbitionApp.SendMessage(GameMessages.LOAD_SCENE, SceneConsts.LOAD_OUT_SCENE);
                    AmbitionApp.SendMessage(GameMessages.SHOW_HEADER);
                }
                else
                {
                    AmbitionApp.SendMessage(GameMessages.LOAD_SCENE, SceneConsts.MAP_SCENE);
                    AmbitionApp.SendMessage(GameMessages.SHOW_HEADER);
                    AmbitionApp.SendMessage(PartyMessages.SHOW_MAP);
                    AmbitionApp.SendMessage(PartyMessages.SELECT_INCIDENTS, partyModel.Incidents);
                }
            }
            else
            {
                AmbitionApp.SendMessage(GameMessages.LOAD_SCENE, SceneConsts.ESTATE_SCENE);
                AmbitionApp.SendMessage(GameMessages.SHOW_HEADER);
            }
            AmbitionApp.SendMessage(GameMessages.FADE_IN);
        }
    }
}
