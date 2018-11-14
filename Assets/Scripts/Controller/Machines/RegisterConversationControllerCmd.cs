using System;
using Core;

namespace Ambition
{
    public class RegisterConversationControllerCmd : ICommand
    {
        public void Execute()
        {
            // In the future, this will be handled by config
            AmbitionApp.RegisterState<StartConversationState>("ConversationController", "InitConversation");
            AmbitionApp.RegisterState("ConversationController", "WaitforFade");
            AmbitionApp.RegisterState<InvokeMachineState, string>("ConversationController", "Incident", "IncidentController");
            AmbitionApp.RegisterState<OpenDialogState, string>("ConversationController", "ReadyGo", ReadyGoDialogMediator.DIALOG_ID);
            AmbitionApp.RegisterState<SendMessageState, string>("ConversationController", "StartConversation", PartyMessages.START_CONVERSATION);
            AmbitionApp.RegisterState<SendMessageState, string>("ConversationController", "FillRemarks", PartyMessages.FILL_REMARKS);

            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("ConversationController", "InitConversation", "WaitforFade", GameMessages.FADE_OUT_COMPLETE);
            AmbitionApp.RegisterLink<CheckIncidentLink>("ConversationController", "WaitforFade", "Incident");
            AmbitionApp.RegisterLink("ConversationController", "WaitforFade", "ReadyGo");
            AmbitionApp.RegisterLink<WaitForCloseDialogLink, string>("ConversationController", "ReadyGo", "StartConversation", ReadyGoDialogMediator.DIALOG_ID);
            AmbitionApp.RegisterLink("ConversationController", "StartConversation", "FillRemarks");


            AmbitionApp.RegisterState<StartRoundState>("ConversationController", "StartRound");
            AmbitionApp.RegisterState("ConversationController", "SelectGuests");
            AmbitionApp.RegisterState<DrinkState>("ConversationController", "Drink");
            AmbitionApp.RegisterState<RoundExpiredState>("ConversationController", "TimeExpired");
            AmbitionApp.RegisterState("ConversationController", "DrawCard");

            AmbitionApp.RegisterLink("ConversationController", "FillRemarks", "StartRound");
            AmbitionApp.RegisterState<SendMessageState, string>("ConversationController", "EndRound", PartyMessages.END_ROUND);
            AmbitionApp.RegisterState<UpdateGuestsState>("ConversationController", "UpdateGuests");
            AmbitionApp.RegisterState<EndConversationState>("ConversationController", "EndConversation");
            AmbitionApp.RegisterState<FleeConversationState>("ConversationController", "FleeConversation");

            AmbitionApp.RegisterLink<SelectGuestLink>("ConversationController", "StartRound", "SelectGuests");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("ConversationController", "StartRound", "Drink", PartyMessages.DRINK);
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("ConversationController", "StartRound", "TimeExpired", PartyMessages.TIME_EXPIRED);
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("ConversationController", "StartRound", "DrawCard", PartyMessages.DRAW_REMARK);

            AmbitionApp.RegisterLink("ConversationController", "DrawCard", "EndRound");
            AmbitionApp.RegisterLink("ConversationController", "SelectGuests", "EndRound");
            AmbitionApp.RegisterLink("ConversationController", "Drink", "EndRound");
            AmbitionApp.RegisterLink("ConversationController", "TimeExpired", "EndRound");
            AmbitionApp.RegisterLink("ConversationController", "EndRound", "UpdateGuests");

            AmbitionApp.RegisterLink<CheckConversationTransition>("ConversationController", "UpdateGuests", "EndConversation");
            AmbitionApp.RegisterLink<CheckRemarksLink>("ConversationController", "UpdateGuests", "FleeConversation");
            AmbitionApp.RegisterLink("ConversationController", "UpdateGuests", "StartRound");

            //These are for handling Incidents after they've been rewarded by Conversations
            AmbitionApp.RegisterState<OpenDialogState, string>("ConversationController", "EndConversationDialogOpen", EndConversationDialogMediator.DIALOG_ID);
            AmbitionApp.RegisterState("ConversationController", "WaitForEndConversationDialogClose");
            
            AmbitionApp.RegisterLink("ConversationController", "EndConversation", "EndConversationDialogOpen");
            AmbitionApp.RegisterLink<WaitForCloseDialogLink, string>("ConversationController", "EndConversationDialogOpen", "WaitForEndConversationDialogClose", EndConversationDialogMediator.DIALOG_ID);
            AmbitionApp.RegisterLink<CheckIncidentLink>("ConversationController", "WaitForEndConversationDialogClose", "Incident");
        }
    }
}
