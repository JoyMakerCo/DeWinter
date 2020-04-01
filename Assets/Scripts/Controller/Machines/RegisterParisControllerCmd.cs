using System;
using Core;
using UFlow;

namespace Ambition
{
    public class RegisterParisControllerCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.RegisterCommand<ChooseExploreLocationsCmd, Pin[]>(ParisMessages.SELECT_DAILIES);
            AmbitionApp.RegisterCommand<ChooseLocationCmd, LocationVO>(ParisMessages.GO_TO_LOCATION);
            AmbitionApp.RegisterCommand<LoadLocationSceneCmd>(ParisMessages.LOAD_LOCATION);

            AmbitionApp.RegisterState<LoadSceneState, string>("ParisMapController", "EnterMap", SceneConsts.PARIS_SCENE);
            AmbitionApp.RegisterState("ParisMapController", "Map");
            AmbitionApp.RegisterState("ParisMapController", "CheckIncident");
            AmbitionApp.RegisterMachineState("ParisMapController", "Incident", "IncidentController");
            AmbitionApp.RegisterState<SendMessageState, string>("ParisMapController", "EnterLocation", ParisMessages.LOAD_LOCATION);
            AmbitionApp.RegisterState("ParisMapController", "Leave");
            AmbitionApp.RegisterState("ParisMapController", "BeginScene");
            AmbitionApp.RegisterState<FadeOutState>("ParisMapController", "ExitMap");

            AmbitionApp.RegisterLink<InputLink, string>("ParisMapController", "EnterMap", "Map", GameMessages.FADE_IN);
            AmbitionApp.RegisterLink<InputLink, string>("ParisMapController", "Map", "Leave", ParisMessages.LEAVE_LOCATION);
            AmbitionApp.RegisterLink<InputLink, string>("ParisMapController", "Map", "Incident", ParisMessages.GO_TO_LOCATION);
            AmbitionApp.RegisterLink<CheckLocationSceneLink>("ParisMapController", "Incident", "EnterLocation");
            AmbitionApp.RegisterLink("ParisMapController", "Incident", "ExitMap");

            AmbitionApp.RegisterLink<FadeInLink>("ParisMapController", "EnterLocation", "BeginScene");
            AmbitionApp.RegisterLink<InputLink, string>("ParisMapController", "BeginScene", "Leave", ParisMessages.LEAVE_LOCATION);
            AmbitionApp.RegisterLink<FadeOutLink>("ParisMapController", "Leave", "ExitMap");
        }
    }
}
