﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Util;

namespace Ambition
{
    public class IncidentTest : MonoBehaviour
    {
        public IncidentConfig Incident;

        // Start is called before the first frame update
        void Awake()
        {
            if (Incident == null) return;
            App.Register<ModelSvc>();
            App.Register<MessageSvc>();
            App.Register<CommandSvc>();
            App.Register<LocalizationSvc>();
            App.Register<UFlow.UFlowSvc>();
            AmbitionApp.Execute<InitGameCmd>();
            GetComponent<InputBlocker>().enabled = true;

            IncidentModel model = AmbitionApp.RegisterModel<IncidentModel>();
            model.Incidents[Incident.name] = Incident.GetIncident();
            model.Schedule(Incident.name);

            GetComponentInChildren<SceneMediator>(true).gameObject.SetActive(true);

            AmbitionApp.Execute<RegisterIncidentControllerCmd, string>(FlowConsts.INCIDENT_CONTROLLER);
            AmbitionApp.GetService<UFlow.UFlowSvc>().InvokeMachine(FlowConsts.INCIDENT_CONTROLLER);
        }
    }
}
