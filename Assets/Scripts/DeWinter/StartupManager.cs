using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace DeWinter
{
	public class StartupManager : MonoBehaviour
	{
		void Awake ()
		{
			DeWinterApp.RegisterModel<GameModel>();
			DeWinterApp.RegisterModel<FactionModel>();
			DeWinterApp.RegisterModel<InventoryModel>();
			DeWinterApp.RegisterModel<ServantModel>();
			DeWinterApp.RegisterModel<PartyModel>();
			DeWinterApp.RegisterModel<CalendarModel>();
			DeWinterApp.RegisterModel<DevotionModel>();
			DeWinterApp.RegisterModel<EventModel>();
			DeWinterApp.RegisterModel<QuestModel>();
			DeWinterApp.RegisterModel<MapModel>();
			DeWinterApp.RegisterModel<LocalizationModel>();

// TODO: Okay, fine, make commands directly executable.
			DeWinterApp.RegisterCommand<SellItemCmd, ItemVO>(InventoryConsts.SELL_ITEM);
			DeWinterApp.RegisterCommand<BuyItemCmd, ItemVO>(InventoryConsts.BUY_ITEM);
			DeWinterApp.RegisterCommand<DancingCmd, NotableVO>(PartyConstants.START_DANCING);
			DeWinterApp.RegisterCommand<GrantRewardCmd, RewardVO>();
			DeWinterApp.RegisterCommand<CheckMilitaryReputationCmd, AdjustValueVO>();
			DeWinterApp.RegisterCommand<GenerateMapCmd, Party>(MapMessage.GENERATE_MAP);
			DeWinterApp.RegisterCommand<DegradeOutfitCmd, Outfit>(InventoryConsts.BUY_ITEM);
			DeWinterApp.RegisterCommand<IntroServantCmd, string>(ServantConsts.INTRODUCE_SERVANT);
			DeWinterApp.RegisterCommand<HireServantCmd, ServantVO>(ServantConsts.HIRE_SERVANT);
			DeWinterApp.RegisterCommand<FireServantCmd, ServantVO>(ServantConsts.FIRE_SERVANT);
			DeWinterApp.RegisterCommand<LoadSceneCmd, string>(GameMessages.LOAD_SCENE);
			DeWinterApp.RegisterCommand<QuitCmd>(GameMessages.QUIT_GAME);
			DeWinterApp.RegisterCommand<NewGameCmd>(GameMessages.NEW_GAME);
			DeWinterApp.RegisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			DeWinterApp.RegisterCommand<GoToRoomCmd, RoomVO>(MapMessage.GO_TO_ROOM);
			DeWinterApp.RegisterCommand<StartPartyCmd>(PartyMessages.START_PARTY);
			DeWinterApp.RegisterCommand<EndEventCmd, EventVO>(EventMessages.END_EVENT);
			DeWinterApp.RegisterCommand<RSVPCmd, Party>(PartyMessages.RSVP);
			DeWinterApp.RegisterCommand<AdvanceDayCmd>(CalendarMessages.NEXT_DAY);
			DeWinterApp.RegisterCommand<SelectDateCmd, DateTime>(CalendarMessages.SELECT_DATE);

			DeWinterApp.RegisterCommand<PayDayCmd, DateTime>();
			DeWinterApp.RegisterCommand<RestockMerchantCmd, DateTime>();
			DeWinterApp.RegisterCommand<CheckUprisingDayCmd, DateTime>();
			DeWinterApp.RegisterCommand<CreateInvitationsCmd, DateTime>();
			DeWinterApp.RegisterCommand<StartEventCmd, DateTime>();
			DeWinterApp.RegisterCommand<CheckStyleChangeCmd, DateTime>();
			DeWinterApp.RegisterCommand<EventReadyCmd, DateTime>();

			Destroy(this.gameObject);
		}
	}
}