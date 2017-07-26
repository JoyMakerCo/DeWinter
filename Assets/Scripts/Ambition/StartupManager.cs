
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Ambition
{
	public class StartupManager : MonoBehaviour
	{
		void Awake ()
		{
			AmbitionApp.RegisterModel<GameModel>();
			AmbitionApp.RegisterModel<FactionModel>();
			AmbitionApp.RegisterModel<InventoryModel>();
			AmbitionApp.RegisterModel<ServantModel>();
			AmbitionApp.RegisterModel<PartyModel>();
			AmbitionApp.RegisterModel<CalendarModel>();
			AmbitionApp.RegisterModel<DevotionModel>();
			AmbitionApp.RegisterModel<EventModel>();
			AmbitionApp.RegisterModel<QuestModel>();
			AmbitionApp.RegisterModel<MapModel>();
			AmbitionApp.RegisterModel<LocalizationModel>();
			AmbitionApp.RegisterModel<OutfitInventoryModel>(); // TODO: Feed this to Inventory

			AmbitionApp.RegisterCommand<SellItemCmd, ItemVO>(InventoryConsts.SELL_ITEM);
			AmbitionApp.RegisterCommand<BuyItemCmd, ItemVO>(InventoryConsts.BUY_ITEM);
			AmbitionApp.RegisterCommand<DancingCmd, NotableVO>(PartyConstants.START_DANCING);
			AmbitionApp.RegisterCommand<GrantRewardCmd, RewardVO>();
			AmbitionApp.RegisterCommand<CheckMilitaryReputationCmd, FactionVO>();
			AmbitionApp.RegisterCommand<GenerateMapCmd, PartyVO>(MapMessage.GENERATE_MAP);
			AmbitionApp.RegisterCommand<DegradeOutfitCmd, Outfit>(InventoryConsts.BUY_ITEM);
			AmbitionApp.RegisterCommand<IntroServantCmd, string>(ServantConsts.INTRODUCE_SERVANT);
			AmbitionApp.RegisterCommand<HireServantCmd, ServantVO>(ServantConsts.HIRE_SERVANT);
			AmbitionApp.RegisterCommand<FireServantCmd, ServantVO>(ServantConsts.FIRE_SERVANT);
			AmbitionApp.RegisterCommand<LoadSceneCmd, string>(GameMessages.LOAD_SCENE);
			AmbitionApp.RegisterCommand<QuitCmd>(GameMessages.QUIT_GAME);
			AmbitionApp.RegisterCommand<NewGameCmd>(GameMessages.NEW_GAME);
			AmbitionApp.RegisterCommand<GoToRoomCmd, RoomVO>(MapMessage.GO_TO_ROOM);
			AmbitionApp.RegisterCommand<StartPartyCmd>(PartyMessages.START_PARTY);
			AmbitionApp.RegisterCommand<EndEventCmd, EventVO>(EventMessages.END_EVENT);
			AmbitionApp.RegisterCommand<RSVPCmd, PartyVO>(PartyMessages.RSVP);
			AmbitionApp.RegisterCommand<AdvanceDayCmd>(CalendarMessages.NEXT_DAY);
			AmbitionApp.RegisterCommand<SelectDateCmd, DateTime>(CalendarMessages.SELECT_DATE);
			AmbitionApp.RegisterCommand<CreateEnemyCmd, string>(GameMessages.CREATE_ENEMY);

			// Party
			AmbitionApp.RegisterCommand<SelectGuestCmd, GuestVO>(PartyMessages.GUEST_SELECTED);
			AmbitionApp.RegisterCommand<AmbushCmd, RoomVO>(PartyMessages.AMBUSH);
			AmbitionApp.RegisterCommand<VictoryCheckCmd, GuestVO[]>();
			AmbitionApp.RegisterCommand<FillHandCmd>(PartyMessages.FILL_REMARKS);
			AmbitionApp.RegisterCommand<FillHandCmd>(PartyMessages.START_ENCOUNTER);
			AmbitionApp.RegisterCommand<AddRemarkCmd>(PartyMessages.ADD_REMARK);
			AmbitionApp.RegisterCommand<BuyRemarkCmd>(PartyMessages.BUY_REMARK);
			AmbitionApp.RegisterCommand<ExchangeRemarkCmd, RemarkVO>(PartyMessages.EXCHANGE_REMARK);
			AmbitionApp.RegisterCommand<EnemyAttackCmd, GuestVO>(PartyMessages.GUEST_SELECTED);
			AmbitionApp.RegisterCommand<DrinkForConfidenceCmd>(PartyMessages.DRINK);
			AmbitionApp.RegisterCommand<SetFashionCmd, PartyVO>(PartyMessages.PARTY_STARTED);
			AmbitionApp.RegisterCommand<FactionTurnModifierCmd, PartyVO>(PartyMessages.PARTY_STARTED);
			AmbitionApp.RegisterCommand<EndTurnCmd>(PartyMessages.END_TURN);
			AmbitionApp.RegisterCommand<HostVictoryCheckCmd>(PartyMessages.END_TURN);
			AmbitionApp.RegisterCommand<FlipRemarkCmd>(PartyMessages.FLIP_REMARK);
			AmbitionApp.RegisterCommand<RoomChoiceCmd, RoomVO>();


			AmbitionApp.RegisterCommand<PayDayCmd, DateTime>();
			AmbitionApp.RegisterCommand<RestockMerchantCmd, DateTime>();
			AmbitionApp.RegisterCommand<CheckUprisingDayCmd, DateTime>();
			AmbitionApp.RegisterCommand<CreateInvitationsCmd, DateTime>();
			AmbitionApp.RegisterCommand<StartEventCmd, DateTime>();
			AmbitionApp.RegisterCommand<CheckStyleChangeCmd, DateTime>();
			AmbitionApp.RegisterCommand<EventReadyCmd, DateTime>();
			AmbitionApp.RegisterCommand<CheckLivreCmd, int>(GameConsts.LIVRE);

			// Initially enabled for TUTORIAL
			AmbitionApp.RegisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			AmbitionApp.RegisterCommand<TutorialPartyWelcomeCmd, RoomVO>();
			AmbitionApp.RegisterCommand<TutorialConfidenceCheckCmd, int>(GameConsts.CONFIDENCE);
			AmbitionApp.RegisterCommand<EndTutorialCmd>(PartyMessages.END_PARTY);

			Destroy(this.gameObject);
		}
	}
}