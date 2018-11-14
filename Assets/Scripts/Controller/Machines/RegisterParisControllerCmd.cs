using System;
using Core;

namespace Ambition
{
    public class RegisterParisControllerCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.RegisterState<LoadSceneState, string>("ParisMapController", "Paris", SceneConsts.PARIS_SCENE);
            AmbitionApp.RegisterState("ParisMapController", "CheckIncident");
            AmbitionApp.RegisterState<InvokeMachineState, string>("ParisMapController", "Incident", "IncidentController");
            AmbitionApp.RegisterState("ParisMapController", "EnterLocation");
            AmbitionApp.RegisterState<SendMessageState, string>("ParisMapController", "LoadLocation", ParisMessages.LOAD_LOCATION);
            AmbitionApp.RegisterState<InvokeMachineState, string>("ParisMapController", "BackOut", "EstateController");
            AmbitionApp.RegisterState<InvokeMachineState, string>("ParisMapController", "Rest", "EstateController");
            AmbitionApp.RegisterState("ParisMapController", "BeginScene");

            AmbitionApp.RegisterLink<ChooseLocationLink>("ParisMapController", "Paris", "CheckIncident");
            AmbitionApp.RegisterLink<CheckIncidentLink>("ParisMapController", "CheckIncident", "Incident");
            AmbitionApp.RegisterLink("ParisMapController", "CheckIncident", "EnterLocation");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("ParisMapController", "Incident", "EnterLocation", IncidentMessages.END_INCIDENTS);
            AmbitionApp.RegisterLink<FadeOutLink>("ParisMapController", "EnterLocation", "LoadLocation");
            AmbitionApp.RegisterLink<FadeInLink>("ParisMapController", "LoadLocation", "BeginScene");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("ParisMapController", "Paris", "Rest", ParisMessages.REST);
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("ParisMapController", "Paris", "BackOut", ParisMessages.ESTATE);
        }
    }
}
