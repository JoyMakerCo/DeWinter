using System;
using Core;
using UFlow;

namespace Ambition
{
    public class RegisterEstateControllerCmd : UFlowConfig
    {
        private string _prev;

        public override void Initialize()
        {
            State("Enter");
            State("ShowEstate");
            State("Invitations");
            State("CheckMissedParties");
            State("Estate");
            State("Exit");

            BindLink<LoadSceneLink, string>("Enter", "ShowEstate", SceneConsts.ESTATE_SCENE);
            BindLink<MessageLink, string>("ShowEstate", "Invitations", GameMessages.FADE_IN_COMPLETE);
            BindLink<MessageLink, string>("Estate", "Exit", EstateMessages.LEAVE_ESTATE);

            Bind<SetActivityState, ActivityType>("ShowEstate", ActivityType.Estate);
            Bind<CreateInvitationsState>("CreateInvitations");
            Bind<CheckMissedPartiesState>("CheckMissedParties");
        }
    }
}
