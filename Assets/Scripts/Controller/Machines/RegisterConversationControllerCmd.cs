﻿using System;
using Core;
using UFlow;

namespace Ambition
{
    public class RegisterConversationControllerCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.RegisterCommand<EndConversationCmd>(PartyMessages.END_CONVERSATION);
            AmbitionApp.RegisterCommand<FleeConversationCmd>(PartyMessages.FLEE_CONVERSATION);
            AmbitionApp.RegisterCommand<InitConversationCmd>(PartyMessages.INIT_CONVERSATION);
            AmbitionApp.RegisterCommand<ExitConversationCmd>(PartyMessages.EXIT_CONVERSATION);

            // CONVERSATION MACHINE
            // In the future, this will be handled by config
            AmbitionApp.RegisterState<SendMessageState, string>("ConversationController", "InitConversation", PartyMessages.INIT_CONVERSATION);
            AmbitionApp.RegisterMachineState("ConversationController", "Incident", "IncidentController");
            AmbitionApp.RegisterState<OpenDialogState, string>("ConversationController", "ReadyGo", ReadyGoDialogMediator.DIALOG_ID);
            AmbitionApp.RegisterState<SendMessageState, string>("ConversationController", "StartConversation", PartyMessages.START_CONVERSATION);
            AmbitionApp.RegisterState<SendMessageState, string>("ConversationController", "FillRemarks", PartyMessages.FILL_REMARKS);

            AmbitionApp.RegisterLink("ConversationController", "InitConversation", "Incident");
            AmbitionApp.RegisterLink("ConversationController", "Incident", "ReadyGo");
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
            AmbitionApp.RegisterState("ConversationController", "CheckRemarks");
            AmbitionApp.RegisterState<SendMessageState, string>("ConversationController", "EndConversation", PartyMessages.END_CONVERSATION);
            AmbitionApp.RegisterState<SendMessageState, string>("ConversationController", "FleeConversation", PartyMessages.FLEE_CONVERSATION);

            AmbitionApp.RegisterLink<GuestSelectedLink>("ConversationController", "StartRound", "SelectGuests");
            AmbitionApp.RegisterLink<MessageLink, string>("ConversationController", "StartRound", "Drink", PartyMessages.DRINK);
            AmbitionApp.RegisterLink<MessageLink, string>("ConversationController", "StartRound", "TimeExpired", PartyMessages.TIME_EXPIRED);
            AmbitionApp.RegisterLink<MessageLink, string>("ConversationController", "StartRound", "DrawCard", PartyMessages.DRAW_REMARK);

            AmbitionApp.RegisterLink("ConversationController", "DrawCard", "EndRound");
            AmbitionApp.RegisterLink("ConversationController", "SelectGuests", "EndRound");
            AmbitionApp.RegisterLink("ConversationController", "Drink", "EndRound");
            AmbitionApp.RegisterLink("ConversationController", "TimeExpired", "EndRound");
            AmbitionApp.RegisterLink("ConversationController", "EndRound", "UpdateGuests");

            //These are for handling Incidents after they've been rewarded by Conversations
            AmbitionApp.RegisterState<OpenDialogState, string>("ConversationController", "EndConversationDialog", EndConversationDialogMediator.DIALOG_ID);
            AmbitionApp.RegisterState<SendMessageState, string>("ConversationController", "FleeConversation", PartyMessages.FLEE_CONVERSATION);
            AmbitionApp.RegisterState<OpenDialogState, string>("ConversationController", "FleeConversationDialog", FleeConversationDialog.DIALOG_ID);
            AmbitionApp.RegisterMachineState("ConversationController", "EndIncident", "IncidentController");
            AmbitionApp.RegisterState("ConversationController", "WaitForEndConversationDialogClose");
            AmbitionApp.RegisterState("ConversationController", "ExitConversationTransition");
            AmbitionApp.RegisterState<SendMessageState, string>("ConversationController", "ExitConversationController", PartyMessages.EXIT_CONVERSATION);

            AmbitionApp.RegisterLink<CheckConversationLink>("ConversationController", "UpdateGuests", "CheckRemarks");
            AmbitionApp.RegisterLink("ConversationController", "UpdateGuests", "EndConversation");
            AmbitionApp.RegisterLink("ConversationController", "EndConversation", "EndConversationDialog");
            AmbitionApp.RegisterLink<CheckRemarksLink>("ConversationController", "CheckRemarks", "StartRound");
            AmbitionApp.RegisterLink("ConversationController", "CheckRemarks", "FleeConversationDialog");
            AmbitionApp.RegisterLink("ConversationController", "FleeConversationDialog", "FleeConversation");

            AmbitionApp.RegisterLink<WaitForCloseDialogLink, string>("ConversationController", "EndConversationDialog", "WaitForEndConversationDialogClose", EndConversationDialogMediator.DIALOG_ID);
            AmbitionApp.RegisterLink<CheckIncidentLink>("ConversationController", "WaitForEndConversationDialogClose", "EndIncident");
            AmbitionApp.RegisterLink("ConversationController", "WaitForEndConversationDialogClose", "ExitConversationTransition");
            AmbitionApp.RegisterLink("ConversationController", "EndIncident", "ExitConversationTransition");
            AmbitionApp.RegisterLink<FadeOutLink>("ConversationController", "ExitConversationTransition", "ExitConversationController");

            AmbitionApp.RegisterLink<WaitForCloseDialogLink, string>("ConversationController", "FleeConversation", "ExitConversationTransition", FleeConversationDialog.DIALOG_ID);
        }
    }
}
