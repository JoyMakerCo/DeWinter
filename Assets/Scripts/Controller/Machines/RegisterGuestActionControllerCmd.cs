using System;
using Core;

namespace Ambition
{
    public class RegisterGuestActionControllerCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.RegisterState("GuestActionController", "GuestActionNone");
            AmbitionApp.RegisterState<GuestActionInterestState>("GuestActionController", "GuestActionInterest");
            AmbitionApp.RegisterState("GuestActionController", "GuestActionComment");
            AmbitionApp.RegisterState("GuestActionController", "GuestActionAside");
            AmbitionApp.RegisterState("GuestActionController", "GuestActionInquiry");
            AmbitionApp.RegisterState("GuestActionController", "GuestActionToast");
            AmbitionApp.RegisterState<GuestActionToastState>("GuestActionController", "GuestActionToastAccepted");
            AmbitionApp.RegisterState("GuestActionController", "GuestActionEnd");
            AmbitionApp.RegisterState<GuestActionSelectLeadReward>("GuestActionController", "GuestActionLead");
            AmbitionApp.RegisterState("GuestActionController", "GuestActionLeadRound");
            AmbitionApp.RegisterState("GuestActionController", "GuestActionLeadUpdate");
            AmbitionApp.RegisterState<GuestActionLeadRewardState>("GuestActionController", "GuestActionLeadReward");
            AmbitionApp.RegisterState<SelectGuestActionState>("GuestActionController", "SelectGuestAction");

            AmbitionApp.RegisterLink("GuestActionController", "GuestActionNone", "GuestActionEnd");
            AmbitionApp.RegisterLink("GuestActionController", "GuestActionInterest", "GuestActionEnd");
            AmbitionApp.RegisterLink("GuestActionController", "GuestActionComment", "GuestActionEnd");
            AmbitionApp.RegisterLink("GuestActionController", "GuestActionAside", "GuestActionEnd");
            AmbitionApp.RegisterLink("GuestActionController", "GuestActionInquiry", "GuestActionEnd");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("GuestActionController", "GuestActionToast", "GuestActionToastAccepted", PartyMessages.DRINK);
            AmbitionApp.RegisterLink("GuestActionController", "GuestActionToastAccepted", "GuestActionEnd");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("GuestActionController", "GuestActionToast", "GuestActionEnd", PartyMessages.END_ROUND);
            AmbitionApp.RegisterLink("GuestActionController", "GuestActionLead", "GuestActionLeadRound");
            AmbitionApp.RegisterLink<GuestActionCheckGuestEngagedLink>("GuestActionController", "GuestActionLeadRound", "GuestActionLeadUpdate");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("GuestActionController", "GuestActionLeadRound", "GuestActionEnd", PartyMessages.END_ROUND);
            AmbitionApp.RegisterLink<GuestActionCheckRoundsLink>("GuestActionController", "GuestActionLeadUpdate", "GuestActionLeadRound");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("GuestActionController", "GuestActionLeadUpdate", "GuestActionLeadReward", PartyMessages.END_ROUND);
            AmbitionApp.RegisterLink("GuestActionController", "GuestActionLeadReward", "GuestActionEnd");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("GuestActionController", "GuestActionEnd", "SelectGuestAction", PartyMessages.END_ROUND);
            AmbitionApp.RegisterLink<GuestActionSelectedLink, string>("GuestActionController", "SelectGuestAction", "GuestActionInterest", "Interest");
            AmbitionApp.RegisterLink<GuestActionSelectedLink, string>("GuestActionController", "SelectGuestAction", "GuestActionToast", "Toast");
            AmbitionApp.RegisterLink<GuestActionSelectedLink, string>("GuestActionController", "SelectGuestAction", "GuestActionLead", "Lead");
            AmbitionApp.RegisterLink("GuestActionController", "SelectGuestAction", "GuestActionNone");
        }
    }
}
