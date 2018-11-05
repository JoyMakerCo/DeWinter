using System;
using Core;
using UFlow;

namespace Ambition
{
    public class RegisterEstateControllerCmd : ICommand
    {
        public void Execute()
        {
            // Estate States. This lands somewhere between confusing and annoying.
            AmbitionApp.RegisterState<LoadSceneState, string>("EstateController", "LoadEstate", SceneConsts.ESTATE_SCENE);
            AmbitionApp.RegisterState<UpdateIncidentState>("EstateController", "UpdateIncidents");
            AmbitionApp.RegisterState("EstateController", "CheckIncident");
            AmbitionApp.RegisterState<InvokeMachineState, string>("EstateController", "Incident", "IncidentController");
            AmbitionApp.RegisterState<StyleChangeState>("EstateController", "StyleChange");
            AmbitionApp.RegisterState("EstateController", "CheckParty");
            AmbitionApp.RegisterState<CreateInvitationsState>("EstateController", "CreateInvitations");
            AmbitionApp.RegisterState<CheckMissedPartiesState>("EstateController", "CheckMissedParties");
            AmbitionApp.RegisterState("EstateController", "Estate");
            AmbitionApp.RegisterState("EstateController", "LeaveEstate");
            AmbitionApp.RegisterState<InvokeMachineState, string>("EstateController", "GoToParty", "PartyController");
            AmbitionApp.RegisterState<LoadSceneState, string>("EstateController", "GoToParis", SceneConsts.PARIS_SCENE);


            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("EstateController", "LoadEstate", "UpdateIncidents", GameMessages.FADE_OUT_COMPLETE);
            AmbitionApp.RegisterLink("EstateController", "UpdateIncidents", "CheckIncident");
            AmbitionApp.RegisterLink<CheckIncidentLink>("EstateController", "CheckIncident", "Incident");
            AmbitionApp.RegisterLink("EstateController", "CheckIncident", "CheckParty");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("EstateController", "Incident", "CheckParty", IncidentMessages.END_INCIDENTS);
            AmbitionApp.RegisterLink<CheckPartyLink>("EstateController", "CheckParty", "GoToParty");
            AmbitionApp.RegisterLink("EstateController", "CheckParty", "CreateInvitations");
            // AmbitionApp.RegisterTransition("EstateController", "Estate", "StyleChange");
            // AmbitionApp.RegisterTransition<WaitForCloseDialogLink>("EstateController", "StyleChange", "CreateInvitations", DialogConsts.MESSAGE);
            AmbitionApp.RegisterLink("EstateController", "CreateInvitations", "CheckMissedParties");
            AmbitionApp.RegisterLink("EstateController", "CheckMissedParties", "Estate");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("EstateController", "Estate", "LeaveEstate", EstateMessages.LEAVE_ESTATE);
            AmbitionApp.RegisterLink<CheckPartyLink>("EstateController", "LeaveEstate", "GoToParty");
            AmbitionApp.RegisterLink("EstateController", "LeaveEstate", "GoToParis");
        }
    }
}
