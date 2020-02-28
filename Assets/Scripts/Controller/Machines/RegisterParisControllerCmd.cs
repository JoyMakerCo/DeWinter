using System;
using Core;
using UFlow;

namespace Ambition
{
    public class RegisterParisControllerCmd : ICommand
    {
        public void Execute()
        {
<<<<<<< Updated upstream
            AmbitionApp.RegisterCommand<ChooseExploreLocationsCmd>(CalendarMessages.UPDATE_CALENDAR);
            AmbitionApp.RegisterCommand<ChooseLocationCmd, Pin>(ParisMessages.GO_TO_LOCATION);
=======
            AmbitionApp.RegisterCommand<ChooseExploreLocationsCmd, Pin[]>(ParisMessages.SELECT_DAILIES);
            AmbitionApp.RegisterCommand<ChooseLocationCmd, LocationVO>(ParisMessages.GO_TO_LOCATION);
>>>>>>> Stashed changes
            AmbitionApp.RegisterCommand<LoadLocationSceneCmd>(ParisMessages.LOAD_LOCATION);
            AmbitionApp.RegisterCommand<UpdateExhaustionCmd>(ParisMessages.UPDATE_EXHAUSTION);

            AmbitionApp.RegisterState<LoadSceneState>("ParisMapController", "EnterMap", SceneConsts.PARIS_SCENE);
            AmbitionApp.RegisterState("ParisMapController", "Map");
            AmbitionApp.RegisterState("ParisMapController", "CheckIncident");
            AmbitionApp.RegisterState<UMachine>("ParisMapController", "Incident", "IncidentController");
            AmbitionApp.RegisterState<SendMessageState>("ParisMapController", "EnterLocation", ParisMessages.LOAD_LOCATION);
            AmbitionApp.RegisterState<MessageInputState>("ParisMapController", "ReturnToEstateInput", ParisMessages.ESTATE);
            AmbitionApp.RegisterState<ReturnToEstateState>("ParisMapController", "ReturnToEstate");
            AmbitionApp.RegisterState<MessageInputState>("ParisMapController", "RestInput", ParisMessages.REST);
            AmbitionApp.RegisterState<RestAtHomeState>("ParisMapController", "Rest");
            AmbitionApp.RegisterState<MessageInputState>("ParisMapController", "LocationInput", ParisMessages.GO_TO_LOCATION);
            AmbitionApp.RegisterState<MessageInputState>("ParisMapController", "Leave", ParisMessages.LEAVE_LOCATION);
            AmbitionApp.RegisterState<MessageInputState>("ParisMapController", "LeaveLocationInput", ParisMessages.LEAVE_LOCATION);
            AmbitionApp.RegisterState<FadeOutState>("ParisMapController", "ExitMap");

<<<<<<< Updated upstream
            AmbitionApp.RegisterLink<MessageLink, string>("ParisMapController", "EnterMap", "ShowHeader", GameMessages.FADE_IN);
            AmbitionApp.RegisterLink("ParisMapController", "ShowHeader", "Map");
            AmbitionApp.RegisterLink<MessageLink, string>("ParisMapController", "Map", "Leave", ParisMessages.LEAVE_LOCATION);
            AmbitionApp.RegisterLink<ChooseLocationLink>("ParisMapController", "Map", "CheckRest");
            AmbitionApp.RegisterLink<RestAtHomeLink>("ParisMapController", "CheckRest", "Rest");
            AmbitionApp.RegisterLink<FadeOutLink>("ParisMapController", "CheckRest", "Incident");
            AmbitionApp.RegisterLink("ParisMapController", "Rest", "Leave");
            AmbitionApp.RegisterLink("ParisMapController", "Incident", "EnterLocation");
            AmbitionApp.RegisterLink<FadeInLink>("ParisMapController", "EnterLocation", "BeginScene");
            AmbitionApp.RegisterLink<MessageLink, string>("ParisMapController", "BeginScene", "Leave", ParisMessages.LEAVE_LOCATION);
            AmbitionApp.RegisterLink<FadeOutLink>("ParisMapController", "Leave", "ExitMap");
=======
            AmbitionApp.RegisterLink("ParisMapController", "EnterMap", "Map");
            AmbitionApp.RegisterLink("ParisMapController", "Map", "Leave");
            AmbitionApp.RegisterLink("ParisMapController", "Map", "ReturnToEstateInput");
            AmbitionApp.RegisterLink("ParisMapController", "ReturnToEstateInput", "ReturnToEstate");
            AmbitionApp.RegisterLink("ParisMapController", "ReturnToEstate", "ExitMap");
            AmbitionApp.RegisterLink("ParisMapController", "Map", "RestInput");
            AmbitionApp.RegisterLink("ParisMapController", "RestInput", "Rest");
            AmbitionApp.RegisterLink("ParisMapController", "Rest", "ExitMap");
            AmbitionApp.RegisterLink("ParisMapController", "Map", "LocationInput");
            AmbitionApp.RegisterLink("ParisMapController", "LocationInput", "Incident");
            AmbitionApp.RegisterLink<CheckLocationSceneLink>("ParisMapController", "Incident", "EnterLocation");
            AmbitionApp.RegisterLink("ParisMapController", "Incident", "ExitMap");

            AmbitionApp.RegisterLink("ParisMapController", "EnterLocation", "LeaveLocationInput");
            AmbitionApp.RegisterLink("ParisMapController", "LeaveLocationInput", "Leave");
            AmbitionApp.RegisterLink("ParisMapController", "Leave", "ExitMap");
>>>>>>> Stashed changes
        }
    }
}
