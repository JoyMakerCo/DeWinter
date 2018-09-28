using System;
using UFlow;
using Core;

namespace Ambition
{
    public class InitGameCmd : ICommand<PlayerConfig>
	{
        public void Execute(PlayerConfig config)
        {
            AmbitionApp.RegisterModel<LocalizationModel>();
			AmbitionApp.RegisterModel<GameModel>();
			AmbitionApp.RegisterModel<FactionModel>();
			AmbitionApp.RegisterModel<InventoryModel>();
			AmbitionApp.RegisterModel<ServantModel>();
			AmbitionApp.RegisterModel<CalendarModel>();
			AmbitionApp.RegisterModel<PartyModel>();
			AmbitionApp.RegisterModel<CharacterModel>();
			AmbitionApp.RegisterModel<QuestModel>();
			AmbitionApp.RegisterModel<MapModel>();
            AmbitionApp.RegisterModel<ConversationModel>();
            AmbitionApp.RegisterModel<ParisModel>();

            // Initialize Selected Player
            AmbitionApp.GetModel<GameModel>().PlayerName = config.name;
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            Array.ForEach(config.Incidents, i => calendar.Schedule(i.Incident));

            AmbitionApp.RegisterCommand<SellItemCmd, ItemVO>(InventoryMessages.SELL_ITEM);
			AmbitionApp.RegisterCommand<BuyItemCmd, ItemVO>(InventoryMessages.BUY_ITEM);
			AmbitionApp.RegisterCommand<GrantRewardCmd, CommodityVO>();
            AmbitionApp.RegisterCommand<GrantRewardsCmd, CommodityVO[]>();
            AmbitionApp.RegisterCommand<CheckMilitaryReputationCmd, FactionVO>();
			AmbitionApp.RegisterCommand<GenerateMapCmd, PartyVO>(MapMessage.GENERATE_MAP);
            AmbitionApp.RegisterCommand<DegradeOutfitCmd>(PartyMessages.END_PARTY);
			AmbitionApp.RegisterCommand<IntroServantCmd, ServantVO>(ServantMessages.INTRODUCE_SERVANT);
			AmbitionApp.RegisterCommand<HireServantCmd, ServantVO>(ServantMessages.HIRE_SERVANT);
			AmbitionApp.RegisterCommand<FireServantCmd, ServantVO>(ServantMessages.FIRE_SERVANT);
			AmbitionApp.RegisterCommand<QuitCmd>(GameMessages.QUIT_GAME);
			AmbitionApp.RegisterCommand<GoToRoomCmd, RoomVO>(MapMessage.GO_TO_ROOM);
            AmbitionApp.RegisterCommand<InvokeMachineCmd, string>(PartyMessages.START_PARTY, "PartyController");
			AmbitionApp.RegisterCommand<UpdatePartyCmd, PartyVO>();
			AmbitionApp.RegisterCommand<SelectDateCmd, DateTime>(CalendarMessages.SELECT_DATE);
			AmbitionApp.RegisterCommand<CreateEnemyCmd, string>(GameMessages.CREATE_ENEMY);
			AmbitionApp.RegisterCommand<AdjustFactionCmd, AdjustFactionVO>(FactionConsts.ADJUST_FACTION);
			AmbitionApp.RegisterCommand<EquipItemCmd, ItemVO>(InventoryMessages.EQUIP);
			AmbitionApp.RegisterCommand<UnequipItemCmd, ItemVO>(InventoryMessages.UNEQUIP);
			AmbitionApp.RegisterCommand<UnequipSlotCmd, string>(InventoryMessages.UNEQUIP);
            AmbitionApp.RegisterCommand<AddLocationCmd, string>(ParisMessages.ADD_LOCATION);
            AmbitionApp.RegisterCommand<RemoveLocationCmd, string>(ParisMessages.REMOVE_LOCATION);
            AmbitionApp.RegisterCommand<StartIncidentCmd, IncidentVO>();
            AmbitionApp.RegisterCommand<GoToPartyCmd, PartyVO>(PartyMessages.GO_TO_PARTY);

            // Party
            AmbitionApp.RegisterCommand<SelectGuestCmd, GuestVO>(PartyMessages.GUEST_SELECTED);
			AmbitionApp.RegisterCommand<AmbushCmd, RoomVO>(PartyMessages.AMBUSH);
			AmbitionApp.RegisterCommand<FillHandCmd>(PartyMessages.FILL_REMARKS);
            AmbitionApp.RegisterCommand<RefillDrinkCmd>(PartyMessages.REFILL_DRINK);
            AmbitionApp.RegisterCommand<RevealRoomCmd, RoomVO>(MapMessage.REVEAL_ROOM);
			AmbitionApp.RegisterCommand<AddRemarkCmd>(PartyMessages.ADD_REMARK);
			AmbitionApp.RegisterCommand<BuyRemarkCmd>(PartyMessages.BUY_REMARK);
			AmbitionApp.RegisterCommand<GuestTargetedCmd, GuestVO>(PartyMessages.GUEST_TARGETED);
			AmbitionApp.RegisterCommand<EnemyAttackCmd, GuestVO>(PartyMessages.GUEST_SELECTED);
			AmbitionApp.RegisterCommand<SetFashionCmd, PartyVO>(PartyMessages.PARTY_STARTED);
			AmbitionApp.RegisterCommand<FactionTurnModifierCmd, PartyVO>(PartyMessages.PARTY_STARTED);
			AmbitionApp.RegisterCommand<RoomChoiceCmd, RoomVO>();
			AmbitionApp.RegisterCommand<EndPartyCmd>(PartyMessages.LEAVE_PARTY);

            AmbitionApp.RegisterCommand<PayDayCmd, DateTime>();
			AmbitionApp.RegisterCommand<RestockMerchantCmd, DateTime>();
			AmbitionApp.RegisterCommand<CheckLivreCmd, int>(GameConsts.LIVRE);

            // Paris
            AmbitionApp.RegisterCommand<RestAtHomeCmd>(ParisMessages.REST);
            AmbitionApp.RegisterCommand<RestockMerchantCmd, DateTime>();

            // Initially enabled for TUTORIAL
            AmbitionApp.RegisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			AmbitionApp.RegisterCommand<SkipTutorialCmd>(GameMessages.SKIP_TUTORIAL);


            // Party Controller
            AmbitionApp.RegisterState<InitPartyGearState>("PartyController", "InitPartyGear");
            AmbitionApp.RegisterState<InitPartyState>("PartyController", "InitParty");
            AmbitionApp.RegisterState<LoadSceneState, string>("PartyController", "PickOutfit", SceneConsts.LOAD_OUT_SCENE);
            AmbitionApp.RegisterState<LoadSceneState, string>("PartyController", "EnterParty", SceneConsts.PARTY_SCENE);
            AmbitionApp.RegisterState<SendMessageState, string>("PartyController", "PartyMap", PartyMessages.SHOW_MAP);
            AmbitionApp.RegisterState<SendMessageState, string>("PartyController", "Conversation", PartyMessages.SHOW_ROOM);
            AmbitionApp.RegisterState<LoadSceneState, string>("PartyController", "AfterPartyResults", SceneConsts.AFTER_PARTY_SCENE);
            AmbitionApp.RegisterState<InvokeMachineState>("PartyController", "ReturnToEstate");
            AmbitionApp.RegisterState<SendMessageState, string>("PartyController", "NextDay", CalendarMessages.NEXT_DAY);

            AmbitionApp.RegisterLink<CheckOutfitLink>("PartyController", "InitPartyGear", "InitParty");
            AmbitionApp.RegisterLink<NoOutfitLink>("PartyController", "InitPartyGear", "PickOutfit");
            AmbitionApp.RegisterLink<WaitForOutfitLink>("PartyController", "PickOutfit", "InitParty");
            AmbitionApp.RegisterLink("PartyController", "InitParty", "EnterParty");
            AmbitionApp.RegisterLink("PartyController", "EnterParty", "PartyMap");
            AmbitionApp.RegisterLink<WaitForRoomLink>("PartyController", "PartyMap", "Conversation");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("PartyController", "Conversation", "PartyMap", PartyMessages.END_CONVERSATION);
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("PartyController", "PartyMap", "AfterPartyResults", PartyMessages.LEAVE_PARTY);
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("PartyController", "AfterPartyResults", "ReturnToEstate", PartyMessages.END_PARTY);
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("PartyController", "ReturnToEstate", "NextDay", GameMessages.FADE_OUT_COMPLETE);

            // In the future, this will be handled by config
            AmbitionApp.RegisterState<StartConversationState>("ConversationController", "InitConversation");
			AmbitionApp.RegisterState<OpenDialogState, string>("ConversationController", "ReadyGo", ReadyGoDialogMediator.DIALOG_ID);
            AmbitionApp.RegisterState<AmbitionMessageState, string>("ConversationController", "StartConversation", PartyMessages.FILL_REMARKS);
            AmbitionApp.RegisterState<StartRoundState>("ConversationController", "StartRound");
			AmbitionApp.RegisterState<DrinkState>("ConversationController", "Drink");
			AmbitionApp.RegisterState<SelectGuestsState>("ConversationController", "SelectGuests");
            AmbitionApp.RegisterState<RoundExpiredState>("ConversationController", "TimeExpired");

			AmbitionApp.RegisterState<EndRoundState>("ConversationController", "EndRound");
			AmbitionApp.RegisterState<EndConversationState>("ConversationController", "EndConversation");
			AmbitionApp.RegisterState<FleeConversationState>("ConversationController", "FleeConversation");

			AmbitionApp.RegisterLink("ConversationController", "InitConversation", "ReadyGo");
            AmbitionApp.RegisterLink<WaitForCloseDialogLink, string>("ConversationController", "ReadyGo", "StartConversation", ReadyGoDialogMediator.DIALOG_ID);
            AmbitionApp.RegisterLink("ConversationController", "StartConversation", "StartRound");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("ConversationController", "StartRound", "Drink", PartyMessages.DRINK);
            AmbitionApp.RegisterLink<SelectGuestsLink>("ConversationController", "StartRound", "SelectGuests");
            AmbitionApp.RegisterLink("ConversationController", "Drink", "EndRound");
            AmbitionApp.RegisterLink("ConversationController", "SelectGuests", "EndRound");
            AmbitionApp.RegisterLink<CheckConversationTransition>("ConversationController", "EndRound", "EndConversation");
            AmbitionApp.RegisterLink<CheckConfidenceLink>("ConversationController", "EndRound", "FleeConversation");
            AmbitionApp.RegisterLink("ConversationController", "EndRound", "StartRound");

			// Estate States. This lands somewhere between confusing and annoying.
			AmbitionApp.RegisterState<LoadSceneState, string>("EstateController", "LoadEstate", SceneConsts.ESTATE_SCENE);
            AmbitionApp.RegisterState<UpdateIncidentState>("EstateController", "UpdateIncidents");
            AmbitionApp.RegisterState("EstateController", "CheckIncident");
            AmbitionApp.RegisterState("EstateController", "WaitForEndIncidents");
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
            AmbitionApp.RegisterLink<CheckIncidentLink>("EstateController", "CheckIncident", "WaitForEndIncidents");
            AmbitionApp.RegisterLink("EstateController", "CheckIncident", "CheckParty");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("EstateController", "WaitForEndIncidents", "CheckParty", IncidentMessages.END_INCIDENTS);
            AmbitionApp.RegisterLink<CheckPartyLink>("EstateController", "CheckParty", "GoToParty");
            AmbitionApp.RegisterLink("EstateController", "CheckParty", "CreateInvitations");
			// AmbitionApp.RegisterTransition("EstateController", "Estate", "StyleChange");
			// AmbitionApp.RegisterTransition<WaitForCloseDialogLink>("EstateController", "StyleChange", "CreateInvitations", DialogConsts.MESSAGE);
			AmbitionApp.RegisterLink("EstateController", "CreateInvitations", "CheckMissedParties");
			AmbitionApp.RegisterLink("EstateController", "CheckMissedParties", "Estate");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("EstateController", "Estate", "LeaveEstate", EstateMessages.LEAVE_ESTATE);
            AmbitionApp.RegisterLink<CheckPartyLink>("EstateController", "LeaveEstate", "GoToParty");
            AmbitionApp.RegisterLink("EstateController", "LeaveEstate", "GoToParis");

            // INCIDENT MACHINE
            AmbitionApp.RegisterState<StartIncidentState>("IncidentController", "StartIncident");
			AmbitionApp.RegisterState<MomentState>("IncidentController", "Moment");
            AmbitionApp.RegisterState<FadeOutState>("IncidentController", "EndIncidentTransition");
            AmbitionApp.RegisterState<EndIncidentState>("IncidentController", "EndIncident");
            AmbitionApp.RegisterState<FadeInState>("IncidentController", "ReturnFromIncident");

            AmbitionApp.RegisterLink("IncidentController", "StartIncident", "Moment");
            AmbitionApp.RegisterLink<CheckEndIncidentLink>("IncidentController", "Moment", "EndIncidentTransition");
            AmbitionApp.RegisterLink<WaitForOptionLink>("IncidentController", "Moment", "Moment");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("IncidentController", "EndIncidentTransition", "EndIncident", GameMessages.FADE_OUT_COMPLETE);
            AmbitionApp.RegisterLink("IncidentController", "EndIncident", "ReturnFromIncident");

            // TUTORIAL STATES.
            AmbitionApp.RegisterState<StartTutorialState>("TutorialController", "InitTutorial");
			AmbitionApp.RegisterState<LoadSceneState, string>("TutorialController", "LoadWardrobeStep", SceneConsts.LOAD_OUT_SCENE);
			AmbitionApp.RegisterState<FadeInState>("TutorialController", "TutorialFadeInWardrobeStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialController", "TutorialWardrobeStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialController", "TutorialGoStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialController", "TutorialPartyStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialController", "TutorialFirstRoomStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialController", "TutorialStartConversationStep");
			AmbitionApp.RegisterState<TutorialRemarkState>("TutorialController", "TutorialRemarkStep");
			AmbitionApp.RegisterState<TutorialGuestState>("TutorialController", "TutorialGuestStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialController", "TutorialAltRemarkStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialController", "TutorialCompleteConversationStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialController", "TutorialPunchbowlStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialController", "TutorialMiddleConversationStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialController", "TutorialHostRoomStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialController", "TutorialHostConversationStep");
			AmbitionApp.RegisterState<EndTutorialState>("TutorialController", "TutorialEndHostConversationStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialController", "TutorialLeaveButtonStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialController", "TutorialLeavePartyStep");

            AmbitionApp.RegisterLink<WaitForSceneLoadedLink, string>("TutorialController", "InitTutorial", "LoadWardrobeStep", SceneConsts.ESTATE_SCENE);
            AmbitionApp.RegisterLink("TutorialController", "LoadWardrobeStep", "TutorialFadeInWardrobeStep");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("TutorialController", "TutorialFadeInWardrobeStep", "TutorialWardrobeStep", GameMessages.FADE_IN_COMPLETE);
            AmbitionApp.RegisterLink<WaitForTutorialStepLink>("TutorialController", "TutorialWardrobeStep", "TutorialGoStep");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("TutorialController", "TutorialGoStep", "TutorialPartyStep", GameMessages.FADE_IN_COMPLETE);
            AmbitionApp.RegisterLink<WaitForCloseDialogLink, string>("TutorialController", "TutorialPartyStep", "TutorialFirstRoomStep", MessageViewMediator.DIALOG_ID);
			AmbitionApp.RegisterLink<WaitForTutorialStepLink>("TutorialController", "TutorialFirstRoomStep", "TutorialStartConversationStep");
			AmbitionApp.RegisterLink<WaitForCloseDialogLink, string>("TutorialController", "TutorialStartConversationStep", "TutorialRemarkStep", ReadyGoDialogMediator.DIALOG_ID);
			AmbitionApp.RegisterLink<TutorialRemarkLink>("TutorialController", "TutorialRemarkStep", "TutorialGuestStep");
			AmbitionApp.RegisterLink<TutorialGuestLink>("TutorialController", "TutorialGuestStep", "TutorialCompleteConversationStep");
			AmbitionApp.RegisterLink<TutorialRemarkLink>("TutorialController", "TutorialGuestStep", "TutorialAltRemarkStep");
			AmbitionApp.RegisterLink("TutorialController", "TutorialAltRemarkStep", "TutorialGuestStep");
			AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("TutorialController", "TutorialCompleteConversationStep", "TutorialPunchbowlStep", PartyMessages.SHOW_MAP);
			AmbitionApp.RegisterLink<WaitForTutorialStepLink>("TutorialController", "TutorialPunchbowlStep", "TutorialMiddleConversationStep");
			AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("TutorialController", "TutorialMiddleConversationStep", "TutorialHostRoomStep", PartyMessages.SHOW_MAP);
			AmbitionApp.RegisterLink<WaitForTutorialStepLink>("TutorialController", "TutorialHostRoomStep", "TutorialHostConversationStep");
			AmbitionApp.RegisterLink<WaitForHostTutorialLink>("TutorialController", "TutorialHostConversationStep", "TutorialEndHostConversationStep");
			AmbitionApp.RegisterLink<WaitForCloseDialogLink, string>("TutorialController", "TutorialEndHostConversationStep", "TutorialLeaveButtonStep", MessageViewMediator.DIALOG_ID);
			AmbitionApp.RegisterLink<WaitForTutorialStepLink>("TutorialController", "TutorialLeaveButtonStep", "TutorialLeavePartyStep");

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

            AmbitionApp.InvokeMachine("EstateController");
        }
	}
}
