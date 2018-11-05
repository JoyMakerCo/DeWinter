using System;
using Core;

namespace Ambition
{
    public class TutorialConversationCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.RegisterState<TutorialState>("TutorialConversationController", "TutorialStart");
            AmbitionApp.RegisterState<TutorialState>("TutorialConversationController", "TutorialMiddleConversationStep");
            AmbitionApp.RegisterState<TutorialState>("TutorialConversationController", "TutorialStartConversationStep");
            AmbitionApp.RegisterState<TutorialRemarkState>("TutorialConversationController", "RemarkTutorial");
            //AmbitionApp.RegisterState<TutorialState>("TutorialConversationController", "AltRemarkTutorial");
            AmbitionApp.RegisterState<TutorialGuestState>("TutorialConversationController", "GuestTutorial");
            AmbitionApp.RegisterState<TutorialState>("TutorialConversationController", "TutorialCompleteConversationStep");
            AmbitionApp.RegisterState<TutorialState>("TutorialConversationController", "GuestActionTutorial");
            AmbitionApp.RegisterState<TutorialState>("TutorialConversationController", "TutorialHostConversationStep");

        }
    }
}
