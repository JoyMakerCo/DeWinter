using System;
using Core;
using UFlow;

namespace Ambition
{
    public class RegisterEstateControllerCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.RegisterCommand<CreateInvitationsCmd>(CalendarMessages.SHOW_INVITATIONS);

            AmbitionApp.RegisterState<LoadSceneState, string>("EstateController", "LoadEstate", SceneConsts.ESTATE_SCENE);
            AmbitionApp.RegisterState<SendMessageState, string>("EstateController", "HideHeader", GameMessages.HIDE_HEADER); // Temp; more states to come
            AmbitionApp.RegisterState<SendMessageState, string>("EstateController", "CreateInvitations", CalendarMessages.SHOW_INVITATIONS);
            AmbitionApp.RegisterState<CheckMissedPartiesState>("EstateController", "CheckMissedParties");
            AmbitionApp.RegisterState("EstateController", "Estate"); // Temp; more states to come
            AmbitionApp.RegisterState("EstateController", "LeaveEstate"); // Temp; more states to come


            AmbitionApp.RegisterLink("EstateController", "LoadEstate", "HideHeader");
            AmbitionApp.RegisterLink<MessageLink, string>("EstateController", "HideHeader", "CreateInvitations", GameMessages.FADE_IN_COMPLETE);
            AmbitionApp.RegisterLink("EstateController", "CreateInvitations", "CheckMissedParties");
            AmbitionApp.RegisterLink("EstateController", "CheckMissedParties", "Estate");
            AmbitionApp.RegisterLink<MessageLink, string>("EstateController", "Estate", "LeaveEstate", EstateMessages.LEAVE_ESTATE);
        }
    }
}
