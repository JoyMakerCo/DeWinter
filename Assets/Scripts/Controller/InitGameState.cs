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
			AmbitionApp.RegisterCommand<TutorialConfidenceCheckCmd>(PartyMessages.SHOW_MAP);
			AmbitionApp.RegisterCommand<EndTutorialCmd>(PartyMessages.SHOW_MAP);


			// UFlow Associations
			// In the future, this will be handled by config
			AmbitionApp.RegisterState<StartConversationState>("InitConversation");
			AmbitionApp.RegisterState<OpenDialogState, string>("ReadyGo", DialogConsts.READY_GO);
			AmbitionApp.RegisterState<StartTurnState>("StartTurn");
			AmbitionApp.RegisterState<EndTurnState>("EndTurn");
			AmbitionApp.RegisterState<EndConversationState>("EndConversation");
			AmbitionApp.RegisterState<FleeConversationState>("FleeConversation");

			AmbitionApp.RegisterTransition("ConversationController", "InitConversation", "ReadyGo");
			AmbitionApp.RegisterTransition<WaitForCloseDialogLink>("ConversationController", "ReadyGo", "StartTurn", DialogConsts.READY_GO);
			AmbitionApp.RegisterTransition<WaitForMessageLink>("ConversationController", "StartTurn", "EndTurn", PartyMessages.END_TURN);
			AmbitionApp.RegisterTransition<CheckConversationTransition>("ConversationController", "EndTurn", "EndConversation");
			AmbitionApp.RegisterTransition<CheckConfidenceLink>("ConversationController", "EndTurn", "FleeConversation");
			AmbitionApp.RegisterTransition("ConversationController", "EndTurn", "StartTurn");

			// Estate States. This lands somewhere between confusing and annoying.
			AmbitionApp.RegisterState<StartEstateState>("InitEstate");
			AmbitionApp.RegisterState<StartIncidentState>("StartEvent");
			AmbitionApp.RegisterState<IncidentState>("EventStage");
			AmbitionApp.RegisterState<CheckEndIncidentState>("CheckEndEvent");
			AmbitionApp.RegisterState<EnterEstateState>("Estate");
			AmbitionApp.RegisterState<StyleChangeState>("StyleChange");
			AmbitionApp.RegisterState<CreateInvitationsState>("CreateInvitations");

			AmbitionApp.RegisterTransition<CheckIncidentsLink>("EstateController", "InitEstate", "StartEvent");
			AmbitionApp.RegisterTransition("EstateController", "InitEstate", "Estate");
			AmbitionApp.RegisterTransition("EstateController", "StartEvent", "EventStage");
			AmbitionApp.RegisterTransition<WaitForIncidentLink>("EstateController", "EventStage", "CheckEndEvent");
			AmbitionApp.RegisterTransition<CheckIncidentsLink>("EstateController", "CheckEndEvent", "EventStage");
			AmbitionApp.RegisterTransition("EstateController", "CheckEndEvent", "Estate");
			// AmbitionApp.RegisterTransition("EstateController", "Estate", "StyleChange");
			// AmbitionApp.RegisterTransition<WaitForCloseDialogLink>("EstateController", "StyleChange", "CreateInvitations", DialogConsts.MESSAGE);
			AmbitionApp.RegisterTransition("EstateController", "Estate", "CreateInvitations");
		}
	}
}
