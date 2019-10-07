using System;
using Core;
using UFlow;

namespace Ambition
{
    public class RegisterParisControllerCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.RegisterCommand<ChooseExploreLocationsCmd>(CalendarMessages.UPDATE_CALENDAR);
            AmbitionApp.RegisterCommand<ChooseLocationCmd, LocationVO>(ParisMessages.GO_TO_LOCATION);
            AmbitionApp.RegisterCommand<LoadLocationSceneCmd>(ParisMessages.LOAD_LOCATION);
            AmbitionApp.RegisterCommand<PopulateParisCmd>(ParisMessages.POPULATE_LOCATIONS);

            AmbitionApp.RegisterState<LoadSceneState, string>("ParisMapController", "EnterMap", SceneConsts.PARIS_SCENE);
            AmbitionApp.RegisterState<SendMessageState, string>("ParisMapController", "ShowHeader", GameMessages.SHOW_HEADER);
            AmbitionApp.RegisterState<SendMessageState, string>("ParisMapController", "Map", ParisMessages.POPULATE_LOCATIONS);
            AmbitionApp.RegisterState("ParisMapController", "CheckRest");
            AmbitionApp.RegisterState("ParisMapController", "CheckIncident");
            AmbitionApp.RegisterMachineState("ParisMapController", "Incident", "IncidentController");
            AmbitionApp.RegisterState<SendMessageState, string>("ParisMapController", "EnterLocation", ParisMessages.LOAD_LOCATION);
            AmbitionApp.RegisterState<RestState>("ParisMapController", "Rest");
            AmbitionApp.RegisterState("ParisMapController", "Leave");
            AmbitionApp.RegisterState("ParisMapController", "BeginScene");
            AmbitionApp.RegisterState<FadeInState>("ParisMapController", "ExitMap");

            AmbitionApp.RegisterLink<MessageLink, string>("ParisMapController", "EnterMap", "ShowHeader", GameMessages.FADE_IN);
            AmbitionApp.RegisterLink("ParisMapController", "ShowHeader", "Map");
            AmbitionApp.RegisterLink<MessageLink, string>("ParisMapController", "Map", "Leave", ParisMessages.LEAVE_LOCATION);
            AmbitionApp.RegisterLink<ChooseLocationLink>("ParisMapController", "Map", "CheckRest");
            AmbitionApp.RegisterLink<RestAtHomeLink>("ParisMapController", "CheckRest", "Rest");
            AmbitionApp.RegisterLink<FadeOutLink>("ParisMapController", "CheckRest", "Incident");
            AmbitionApp.RegisterLink("ParisMapController", "Rest", "Leave");
            AmbitionApp.RegisterLink<CheckLocationSceneLink>("ParisMapController", "Incident", "EnterLocation");
            AmbitionApp.RegisterLink("ParisMapController", "Incident", "ExitMap");

            AmbitionApp.RegisterLink<FadeInLink>("ParisMapController", "EnterLocation", "BeginScene");
            AmbitionApp.RegisterLink<MessageLink, string>("ParisMapController", "BeginScene", "Leave", ParisMessages.LEAVE_LOCATION);
            AmbitionApp.RegisterLink<FadeOutLink>("ParisMapController", "Leave", "ExitMap");
        }
    }
}
