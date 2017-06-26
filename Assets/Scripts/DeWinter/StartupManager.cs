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
			Ambition.RegisterModel<GameModel>();
			Ambition.RegisterModel<FactionModel>();
			Ambition.RegisterModel<InventoryModel>();
			Ambition.RegisterModel<ServantModel>();
			Ambition.RegisterModel<PartyModel>();
			Ambition.RegisterModel<CalendarModel>();
			Ambition.RegisterModel<DevotionModel>();
			Ambition.RegisterModel<EventModel>();
			Ambition.RegisterModel<QuestModel>();
			Ambition.RegisterModel<MapModel>();
			Ambition.RegisterModel<LocalizationModel>();

// TODO: Okay, fine, make commands directly executable.
			Ambition.RegisterCommand<SellItemCmd, ItemVO>(InventoryConsts.SELL_ITEM);
			Ambition.RegisterCommand<BuyItemCmd, ItemVO>(InventoryConsts.BUY_ITEM);
			Ambition.RegisterCommand<DancingCmd, NotableVO>(PartyConstants.START_DANCING);
			Ambition.RegisterCommand<GrantRewardCmd, RewardVO>();
			Ambition.RegisterCommand<CheckMilitaryReputationCmd, AdjustValueVO>();
			Ambition.RegisterCommand<GenerateMapCmd, Party>(MapMessage.GENERATE_MAP);
			Ambition.RegisterCommand<DegradeOutfitCmd, Outfit>(InventoryConsts.BUY_ITEM);
			Ambition.RegisterCommand<IntroServantCmd, string>(ServantConsts.INTRODUCE_SERVANT);
			Ambition.RegisterCommand<HireServantCmd, ServantVO>(ServantConsts.HIRE_SERVANT);
			Ambition.RegisterCommand<FireServantCmd, ServantVO>(ServantConsts.FIRE_SERVANT);
			Ambition.RegisterCommand<LoadSceneCmd, string>(GameMessages.LOAD_SCENE);
			Ambition.RegisterCommand<QuitCmd>(GameMessages.QUIT_GAME);
			Ambition.RegisterCommand<NewGameCmd>(GameMessages.NEW_GAME);
			Ambition.RegisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			Ambition.RegisterCommand<GoToRoomCmd, RoomVO>(MapMessage.GO_TO_ROOM);
			Ambition.RegisterCommand<StartPartyCmd>(PartyMessages.START_PARTY);
			Ambition.RegisterCommand<EndEventCmd, EventVO>(EventMessages.END_EVENT);
			Ambition.RegisterCommand<RSVPCmd, Party>(PartyMessages.RSVP);
			Ambition.RegisterCommand<AdvanceDayCmd>(CalendarMessages.NEXT_DAY);
			Ambition.RegisterCommand<SelectDateCmd, DateTime>(CalendarMessages.SELECT_DATE);

			Ambition.RegisterCommand<PayDayCmd, DateTime>();
			Ambition.RegisterCommand<RestockMerchantCmd, DateTime>();
			Ambition.RegisterCommand<CheckUprisingDayCmd, DateTime>();
			Ambition.RegisterCommand<CreateInvitationsCmd, DateTime>();
			Ambition.RegisterCommand<StartEventCmd, DateTime>();
			Ambition.RegisterCommand<CheckStyleChangeCmd, DateTime>();
			Ambition.RegisterCommand<EventReadyCmd, DateTime>();

			Destroy(this.gameObject);
		}
	}
}