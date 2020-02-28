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

            AmbitionApp.RegisterState<LoadSceneState>("EstateController", "LoadEstate", SceneConsts.ESTATE_SCENE);
            AmbitionApp.RegisterState<SendMessageState>("EstateController", "HideHeader", GameMessages.HIDE_HEADER); // Temp; more states to come
            AmbitionApp.RegisterState<MessageInputState>("EstateController", "FadeComplete", GameMessages.FADE_IN_COMPLETE); // Temp; more states to come
            AmbitionApp.RegisterState<SendMessageState>("EstateController", "CreateInvitations", CalendarMessages.SHOW_INVITATIONS);
            AmbitionApp.RegisterState<CheckMissedPartiesState>("EstateController", "CheckMissedParties");
            AmbitionApp.RegisterState<MessageInputState>("EstateController", "Estate", EstateMessages.LEAVE_ESTATE); // Temp; more states to come
            AmbitionApp.RegisterState("EstateController", "LeaveEstate"); // Temp; more states to come


            AmbitionApp.RegisterLink("EstateController", "LoadEstate", "HideHeader");
            AmbitionApp.RegisterLink("EstateController", "HideHeader", "FadeComplete");
            AmbitionApp.RegisterLink("EstateController", "FadeComplete", "CreateInvitations");
            AmbitionApp.RegisterLink("EstateController", "CreateInvitations", "CheckMissedParties");
            AmbitionApp.RegisterLink("EstateController", "CheckMissedParties", "Estate");
            AmbitionApp.RegisterLink("EstateController", "Estate", "LeaveEstate");
        }
    }
}
