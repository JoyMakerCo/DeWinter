using System;
using UFlow;
using Core;

namespace Ambition
{
	public class InitGameState : UState
	{
		public override void OnEnterState ()
		{
			AmbitionApp.RegisterModel<GameModel>();
			AmbitionApp.RegisterModel<FactionModel>();
			AmbitionApp.RegisterModel<InventoryModel>();
			AmbitionApp.RegisterModel<ServantModel>();
			AmbitionApp.RegisterModel<CalendarModel>();
			AmbitionApp.RegisterModel<PartyModel>();
			AmbitionApp.RegisterModel<CharacterModel>();
			AmbitionApp.RegisterModel<IncidentModel>();
			AmbitionApp.RegisterModel<QuestModel>();
			AmbitionApp.RegisterModel<MapModel>();
			AmbitionApp.RegisterModel<LocalizationModel>();

			AmbitionApp.RegisterCommand<SellItemCmd, ItemVO>(InventoryMessages.SELL_ITEM);
			AmbitionApp.RegisterCommand<BuyItemCmd, ItemVO>(InventoryMessages.BUY_ITEM);
			AmbitionApp.RegisterCommand<DancingCmd, NotableVO>(PartyConstants.START_DANCING);
			AmbitionApp.RegisterCommand<GrantRewardCmd, CommodityVO>();
			AmbitionApp.RegisterCommand<CheckMilitaryReputationCmd, FactionVO>();
			AmbitionApp.RegisterCommand<GenerateMapCmd, PartyVO>(MapMessage.GENERATE_MAP);
			AmbitionApp.RegisterCommand<DegradeOutfitCmd, OutfitVO>(InventoryMessages.BUY_ITEM);
			AmbitionApp.RegisterCommand<IntroServantCmd, ServantVO>(ServantMessages.INTRODUCE_SERVANT);
			AmbitionApp.RegisterCommand<HireServantCmd, ServantVO>(ServantMessages.HIRE_SERVANT);
			AmbitionApp.RegisterCommand<FireServantCmd, ServantVO>(ServantMessages.FIRE_SERVANT);
			AmbitionApp.RegisterCommand<QuitCmd>(GameMessages.QUIT_GAME);
			AmbitionApp.RegisterCommand<GoToRoomCmd, RoomVO>(MapMessage.GO_TO_ROOM);
			AmbitionApp.RegisterCommand<StartPartyCmd>(PartyMessages.START_PARTY);
			AmbitionApp.RegisterCommand<CalculateConfidenceCmd>(PartyMessages.START_PARTY);
			AmbitionApp.RegisterCommand<RSVPCmd, PartyVO>(PartyMessages.RSVP);
			AmbitionApp.RegisterCommand<AdvanceDayCmd>(CalendarMessages.NEXT_DAY);
			AmbitionApp.RegisterCommand<SelectDateCmd, DateTime>(CalendarMessages.SELECT_DATE);
			AmbitionApp.RegisterCommand<CreateEnemyCmd, string>(GameMessages.CREATE_ENEMY);
			AmbitionApp.RegisterCommand<AdjustFactionCmd, AdjustFactionVO>(FactionConsts.ADJUST_FACTION);
			AmbitionApp.RegisterCommand<EquipItemCmd, ItemVO>(InventoryMessages.EQUIP);
			AmbitionApp.RegisterCommand<UnequipItemCmd, ItemVO>(InventoryMessages.UNEQUIP);
			AmbitionApp.RegisterCommand<UnequipSlotCmd, string>(InventoryMessages.UNEQUIP);

			// Party
			AmbitionApp.RegisterCommand<SelectGuestCmd, GuestVO>(PartyMessages.GUEST_SELECTED);
			AmbitionApp.RegisterCommand<AmbushCmd, RoomVO>(PartyMessages.AMBUSH);
			AmbitionApp.RegisterCommand<FillHandCmd>(PartyMessages.FILL_REMARKS);
			AmbitionApp.RegisterCommand<AddRemarkCmd>(PartyMessages.ADD_REMARK);
			AmbitionApp.RegisterCommand<BuyRemarkCmd>(PartyMessages.BUY_REMARK);
			AmbitionApp.RegisterCommand<GuestTargetedCmd, GuestVO>(PartyMessages.GUEST_TARGETED);
			AmbitionApp.RegisterCommand<EnemyAttackCmd, GuestVO>(PartyMessages.GUEST_SELECTED);
			AmbitionApp.RegisterCommand<DrinkForConfidenceCmd>(PartyMessages.DRINK);
			AmbitionApp.RegisterCommand<SetFashionCmd, PartyVO>(PartyMessages.PARTY_STARTED);
			AmbitionApp.RegisterCommand<FactionTurnModifierCmd, PartyVO>(PartyMessages.PARTY_STARTED);
			AmbitionApp.RegisterCommand<RoomChoiceCmd, RoomVO>();
			AmbitionApp.RegisterCommand<EndPartyCmd>(PartyMessages.LEAVE_PARTY);

			AmbitionApp.RegisterCommand<PayDayCmd, DateTime>();
			AmbitionApp.RegisterCommand<RestockMerchantCmd, DateTime>();
			AmbitionApp.RegisterCommand<CheckUprisingDayCmd, DateTime>();
			AmbitionApp.RegisterCommand<CheckLivreCmd, int>(GameConsts.LIVRE);

			// Initially enabled for TUTORIAL
			AmbitionApp.RegisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			AmbitionApp.RegisterCommand<SkipTutorialCmd>(GameMessages.SKIP_TUTORIAL);

			// UFlow Associations
			// In the future, this will be handled by config
			AmbitionApp.RegisterState<StartConversationState>("ConversationController", "InitConversation");
			AmbitionApp.RegisterState<OpenDialogState, string>("ConversationController", "ReadyGo", ReadyGoDialogMediator.DIALOG_ID);
			AmbitionApp.RegisterState<StartTurnState>("ConversationController", "StartTurn");
			AmbitionApp.RegisterState<EndTurnState>("ConversationController", "EndTurn");
			AmbitionApp.RegisterState<EndConversationState>("ConversationController", "EndConversation");
			AmbitionApp.RegisterState<FleeConversationState>("ConversationController", "FleeConversation");

			AmbitionApp.RegisterLink("ConversationController", "InitConversation", "ReadyGo");
			AmbitionApp.RegisterLink<WaitForCloseDialogLink, string>("ConversationController", "ReadyGo", "StartTurn", ReadyGoDialogMediator.DIALOG_ID);
			AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("ConversationController", "StartTurn", "EndTurn", PartyMessages.END_TURN);
			AmbitionApp.RegisterLink<CheckConversationTransition>("ConversationController", "EndTurn", "EndConversation");
			AmbitionApp.RegisterLink<CheckConfidenceLink>("ConversationController", "EndTurn", "FleeConversation");
			AmbitionApp.RegisterLink("ConversationController", "EndTurn", "StartTurn");

			// Estate States. This lands somewhere between confusing and annoying.
			AmbitionApp.RegisterState<LoadSceneState, string>("EstateController", "LoadEstate", SceneConsts.ESTATE_SCENE);
			AmbitionApp.RegisterState<StartEstateState>("EstateController", "InitEstate");
			AmbitionApp.RegisterState<FadeInState>("EstateController", "EnterEstate");
			AmbitionApp.RegisterState<StyleChangeState>("EstateController", "StyleChange");
			AmbitionApp.RegisterState<CreateInvitationsState>("EstateController", "CreateInvitations");
			AmbitionApp.RegisterState<CheckMissedPartiesState>("EstateController", "CheckMissedParties");
			AmbitionApp.RegisterState("EstateController", "Estate");

			AmbitionApp.RegisterLink("EstateController", "LoadEstate", "InitEstate");
			AmbitionApp.RegisterLink("EstateController", "InitEstate", "EnterEstate");
			AmbitionApp.RegisterLink("EstateController", "EnterEstate", "CreateInvitations");
			// AmbitionApp.RegisterTransition("EstateController", "Estate", "StyleChange");
			// AmbitionApp.RegisterTransition<WaitForCloseDialogLink>("EstateController", "StyleChange", "CreateInvitations", DialogConsts.MESSAGE);
			AmbitionApp.RegisterLink("EstateController", "CreateInvitations", "CheckMissedParties");
			AmbitionApp.RegisterLink("EstateController", "CheckMissedParties", "Estate");

			// INCIDENT MACHINE
			AmbitionApp.RegisterState<LoadSceneState, string>("IncidentController", "LoadIncident", SceneConsts.INCIDENT_SCENE);
			AmbitionApp.RegisterState<FadeInState>("IncidentController", "EnterIncident");
			AmbitionApp.RegisterState<StartIncidentState>("IncidentController", "StartIncident");
			AmbitionApp.RegisterState<MomentState>("IncidentController", "Moment");
			AmbitionApp.RegisterState("IncidentController", "CheckEndIncident");
			AmbitionApp.RegisterState<FadeOutState>("IncidentController", "EndIncident");
			AmbitionApp.RegisterState<InvokeMachineState, string>("IncidentController", "InvokeEstate", "EstateController");

			AmbitionApp.RegisterLink("IncidentController", "LoadIncident", "EnterIncident");
			AmbitionApp.RegisterLink("IncidentController", "EnterIncident", "StartIncident");
			AmbitionApp.RegisterLink("IncidentController", "StartIncident", "Moment");
			AmbitionApp.RegisterLink<WaitForMomentLink>("IncidentController", "Moment", "CheckEndIncident");
			AmbitionApp.RegisterLink<CheckEndIncidentLink>("IncidentController", "CheckEndIncident", "EndIncident");
			AmbitionApp.RegisterLink("IncidentController", "CheckEndIncident", "Moment");
			AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("IncidentController", "EndIncident", "InvokeEstate", GameMessages.FADE_OUT_COMPLETE);


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
			AmbitionApp.RegisterState<GuestActionLeadState>("GuestActionController", "GuestActionLead");
			AmbitionApp.RegisterState<SelectGuestActionState>("GuestActionController", "SelectGuestAction");

			AmbitionApp.RegisterLink("GuestActionController", "GuestActionNone", "GuestActionEnd");
			AmbitionApp.RegisterLink("GuestActionController", "GuestActionInterest", "GuestActionEnd");
			AmbitionApp.RegisterLink("GuestActionController", "GuestActionComment", "GuestActionEnd");
			AmbitionApp.RegisterLink("GuestActionController", "GuestActionAside", "GuestActionEnd");
			AmbitionApp.RegisterLink("GuestActionController", "GuestActionInquiry", "GuestActionEnd");
			AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("GuestActionController", "GuestActionToast", "GuestActionToastAccepted", PartyMessages.DRINK);
			AmbitionApp.RegisterLink("GuestActionController", "GuestActionToastAccepted", "GuestActionEnd");
			AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("GuestActionController", "GuestActionToast", "GuestActionEnd", PartyMessages.END_TURN);
			AmbitionApp.RegisterLink("GuestActionController", "GuestActionLead", "GuestActionEnd");
			AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("GuestActionController", "GuestActionEnd", "SelectGuestAction", PartyMessages.START_TURN);
			AmbitionApp.RegisterLink<GuestActionSelectedLink, string>("GuestActionController", "SelectGuestAction", "GuestActionInterest", "Interest");
			AmbitionApp.RegisterLink("GuestActionController", "SelectGuestAction", "GuestActionNone");
		}
	}
}
