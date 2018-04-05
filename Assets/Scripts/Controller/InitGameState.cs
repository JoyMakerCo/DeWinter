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
			AmbitionApp.RegisterCommand<LoadSceneCmd, string>(GameMessages.LOAD_SCENE);
			AmbitionApp.RegisterCommand<QuitCmd>(GameMessages.QUIT_GAME);
			AmbitionApp.RegisterCommand<GoToRoomCmd, RoomVO>(MapMessage.GO_TO_ROOM);
			AmbitionApp.RegisterCommand<StartPartyCmd>(PartyMessages.START_PARTY);
			AmbitionApp.RegisterCommand<CalculateConfidenceCmd>(PartyMessages.START_PARTY);
			AmbitionApp.RegisterCommand<RSVPCmd, PartyVO>(PartyMessages.RSVP);
			AmbitionApp.RegisterCommand<AdvanceDayCmd>(CalendarMessages.NEXT_DAY);
			AmbitionApp.RegisterCommand<SelectDateCmd, DateTime>(CalendarMessages.SELECT_DATE);
			AmbitionApp.RegisterCommand<CreateEnemyCmd, string>(GameMessages.CREATE_ENEMY);
			AmbitionApp.RegisterCommand<AdjustFactionCmd, AdjustFactionVO>(FactionConsts.ADJUST_FACTION);
			AmbitionApp.RegisterCommand<EquipItemCmd, ItemVO>(InventoryMessages.EQUIP_ITEM);
			AmbitionApp.RegisterCommand<UnequipItemCmd, ItemVO>(InventoryMessages.UNEQUIP_ITEM);
			AmbitionApp.RegisterCommand<UnequipSlotCmd, string>(InventoryMessages.UNEQUIP_ITEM);

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
			AmbitionApp.RegisterState<StartConversationState>("InitConversation");
			AmbitionApp.RegisterState<OpenDialogState, string>("ReadyGo", ReadyGoDialogMediator.DIALOG_ID);
			AmbitionApp.RegisterState<StartTurnState>("StartTurn");
			AmbitionApp.RegisterState<EndTurnState>("EndTurn");
			AmbitionApp.RegisterState<EndConversationState>("EndConversation");
			AmbitionApp.RegisterState<FleeConversationState>("FleeConversation");

			AmbitionApp.RegisterLink("ConversationController", "InitConversation", "ReadyGo");
			AmbitionApp.RegisterLink<WaitForCloseDialogLink>("ConversationController", "ReadyGo", "StartTurn", ReadyGoDialogMediator.DIALOG_ID);
			AmbitionApp.RegisterLink<WaitForMessageLink>("ConversationController", "StartTurn", "EndTurn", PartyMessages.END_TURN);
			AmbitionApp.RegisterLink<CheckConversationTransition>("ConversationController", "EndTurn", "EndConversation");
			AmbitionApp.RegisterLink<CheckConfidenceLink>("ConversationController", "EndTurn", "FleeConversation");
			AmbitionApp.RegisterLink("ConversationController", "EndTurn", "StartTurn");

			// Estate States. This lands somewhere between confusing and annoying.
			AmbitionApp.RegisterState<StartEstateState>("InitEstate");
			AmbitionApp.RegisterState<StartIncidentState>("StartEvent");
			AmbitionApp.RegisterState<IncidentState>("EventStage");
			AmbitionApp.RegisterState<CheckEndIncidentState>("CheckEndEvent");
			AmbitionApp.RegisterState<EnterEstateState>("Estate");
			AmbitionApp.RegisterState<StyleChangeState>("StyleChange");
			AmbitionApp.RegisterState<CreateInvitationsState>("CreateInvitations");

			AmbitionApp.RegisterLink<CheckIncidentsLink>("EstateController", "InitEstate", "StartEvent");
			AmbitionApp.RegisterLink("EstateController", "InitEstate", "Estate");
			AmbitionApp.RegisterLink("EstateController", "StartEvent", "EventStage");
			AmbitionApp.RegisterLink<WaitForIncidentLink>("EstateController", "EventStage", "CheckEndEvent");
			AmbitionApp.RegisterLink<CheckIncidentsLink>("EstateController", "CheckEndEvent", "EventStage");
			AmbitionApp.RegisterLink("EstateController", "CheckEndEvent", "Estate");
			// AmbitionApp.RegisterTransition("EstateController", "Estate", "StyleChange");
			// AmbitionApp.RegisterTransition<WaitForCloseDialogLink>("EstateController", "StyleChange", "CreateInvitations", DialogConsts.MESSAGE);
			AmbitionApp.RegisterLink("EstateController", "Estate", "CreateInvitations");

			// TUTORIAL STATES.
			AmbitionApp.RegisterState<StartTutorialState>("InitTutorial");
			AmbitionApp.RegisterState<TutorialState>("TutorialWardrobeStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialGoStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialPartyStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialFirstRoomStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialStartConversationStep");
			AmbitionApp.RegisterState<TutorialRemarkState>("TutorialRemarkStep");
			AmbitionApp.RegisterState<TutorialGuestState>("TutorialGuestStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialAltRemarkStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialCompleteConversationStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialPunchbowlStep");
			AmbitionApp.RegisterState<TutorialState>("TutorialHostRoomStep");
			AmbitionApp.RegisterState<EndTutorialState>("EndTutorial");

			AmbitionApp.RegisterLink("TutorialController", "InitTutorial", "TutorialWardrobeStep");
			AmbitionApp.RegisterLink<WaitForTutorialStepLink>("TutorialController", "TutorialWardrobeStep", "TutorialGoStep");
			AmbitionApp.RegisterLink<WaitForTutorialStepLink>("TutorialController", "TutorialGoStep", "TutorialPartyStep");
			AmbitionApp.RegisterLink<WaitForCloseDialogLink>("TutorialController", "TutorialPartyStep", "TutorialFirstRoomStep", MessageViewMediator.DIALOG_ID);
			AmbitionApp.RegisterLink<WaitForTutorialStepLink>("TutorialController", "TutorialFirstRoomStep", "TutorialStartConversationStep");
			AmbitionApp.RegisterLink<WaitForCloseDialogLink>("TutorialController", "TutorialStartConversationStep", "TutorialRemarkStep", ReadyGoDialogMediator.DIALOG_ID);
			AmbitionApp.RegisterLink<TutorialRemarkLink>("TutorialController", "TutorialRemarkStep", "TutorialGuestStep");
			AmbitionApp.RegisterLink<TutorialGuestLink>("TutorialController", "TutorialGuestStep", "TutorialCompleteConversationStep");
			AmbitionApp.RegisterLink<TutorialRemarkLink>("TutorialController", "TutorialGuestStep", "TutorialAltRemarkStep");
			AmbitionApp.RegisterLink("TutorialController", "TutorialAltRemarkStep", "TutorialGuestStep");
			AmbitionApp.RegisterLink<WaitForMessageLink>("TutorialController", "TutorialCompleteConversationStep", "TutorialPunchbowlStep", PartyMessages.SHOW_MAP);
			AmbitionApp.RegisterLink<WaitForMessageLink>("TutorialController", "TutorialPunchbowlStep", "TutorialHostRoomStep", PartyMessages.SHOW_MAP);
			AmbitionApp.RegisterLink<WaitForMessageLink>("TutorialController", "TutorialHostRoomStep", "TutorialLeavePartyStep", PartyMessages.SHOW_MAP);
			AmbitionApp.RegisterLink<WaitForMessageLink>("TutorialController", "HostTutorial", "EndTutorial", PartyMessages.LEAVE_PARTY);
		}
	}
}
