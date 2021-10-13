using System;
using Core;
using UFlow;

namespace Ambition
{
    public class ParisFlow : UFlow.UFlowConfig
    {
        public override void Configure()
        {
            State("Paris");
            State("Map");
            State("MapLocationInput");
            State("Go to Location");
            State("Location Incident");
            State("Location Scene Decision");
            State("Location Scene");
            State("Leave Location Input");
            State("Complete Location");
            State("Exit");

            State("MapEstateInput", false);
            State("Return To Estate");

            Link("Map", "MapEstateInput");
            Link("MapLeaveInput", "Exit");
            Link("Location Incident", "Exit");
            Link("Location Scene Decision", "Complete Location");

            Decision("Location Scene Decision", () => !string.IsNullOrWhiteSpace(AmbitionApp.Paris.GetLocation()?.SceneID));

            Bind<PopulateParisState>("Map");
            Bind<PickLocationState>("Go to Location");
            Bind<UMachine>("Location Incident", FlowConsts.INCIDENT_CONTROLLER);
            Bind<ReturnToEstateState>("Return To Estate");
            Bind<CompleteLocationState>("Complete Location");
            Bind<LoadLocationSceneInput>("Location Scene");

            Bind<LoadSceneInput, string>("Paris", SceneConsts.PARIS_SCENE);
            Bind<MessageInput, string>("MapLocationInput", ParisMessages.CHOOSE_LOCATION);
            Bind<MessageInput, string>("MapEstateInput", ParisMessages.ESTATE);
            Bind<MessageInput, string>("Leave Location Input", ParisMessages.LEAVE_LOCATION);
        }
    }
}
