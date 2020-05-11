using System;
using Core;
using UFlow;

namespace Ambition
{
    public class RegisterParisControllerCmd : UFlow.UFlowConfig
    {
        public override void Initialize()
        {
            State("EnterMap");
            State("Map");
            State("Go to Location");
            State("Location Incident");
            State("Location Scene Decision");
            State("Location Scene"); 
            State("Exit");
            State("Return To Estate", false);

            Link("Map", "Exit");
            Link("Map", "Return To Estate");
            Link("Return To Estate", "Exit");
            Link("Location Incident", "Exit");

            Bind<PickLocationState>("Go to Location");
            Bind<UMachine, string>("Location Incident", FlowConsts.INCIDENT_CONTROLLER);
            Bind<ReturnToEstateState>("Return To Estate");

            BindLink<LoadSceneLink, string>("EnterMap", "Map", SceneConsts.PARIS_SCENE);
            BindLink<MessageLink, string>("Map", "Go to Location", ParisMessages.GO_TO_LOCATION);
            BindLink<MessageLink, string>("Map", "Exit", ParisMessages.LEAVE_LOCATION);
            BindLink<MessageLink, string>("Map", "Return To Estate", ParisMessages.ESTATE);
            BindLink<CheckLocationSceneLink>("Location Incident", "Location Scene Decision");
            BindLink<LoadLocationLink>("Location Scene Decision", "Location Scene");
            BindLink<MessageLink, string>("Location Scene", "Exit", ParisMessages.LEAVE_LOCATION);
        }
    }
}
