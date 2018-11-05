using System;
using Core;

namespace Ambition
{
    public class RegisterParisControllerCmd : ICommand
    {
        public void Execute()
        {
            // PARIS STATES.
            //AmbitionApp.RegisterState("ParisMapController", "EnterParisState");
            //AmbitionApp.RegisterState<FadeOutState>("ParisMapController", "LeaveParisMapState");
            //AmbitionApp.RegisterState("ParisMapController", "LoadIncidentDecisionState");

            //AmbitionApp.RegisterState<LoadParisIncidentState>("ParisMapController", "LoadIncidentState");
            //AmbitionApp.RegisterState<UMachine>("ParisMapController", "IncidentController");
            //AmbitionApp.RegisterState<ParisLocationState>("ParisMapController", "ParisLocationState");

            //AmbitionApp.RegisterState<RestAtHomeState>("ParisMapController", "RestAtHomeState");
            //AmbitionApp.RegisterState<LoadSceneState, string>("ParisMapController", "GoHomeState", SceneConsts.ESTATE_SCENE);
            //AmbitionApp.RegisterState<NextDayState>("ParisMapController", "NextDayState");

            //AmbitionApp.RegisterLink<ChooseLocationLink>("ParisMapController", "EnterParisState", "LeaveParisMapState");
            //AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("ParisMapController", "LeaveParisMapState", "LoadIncidentDecisionState", GameMessages.FADE_OUT_COMPLETE);
            ////AmbitionApp.RegisterLink<ValidateParisIncidentState>("ParisMapController", "LoadIncidentDecisionState", "LoadIncidentState", GameMessages.FADE_OUT_COMPLETE);


            //AmbitionApp.RegisterLink("ParisMapController", "LoadEstate", "NextDayState");
        }
    }
}
