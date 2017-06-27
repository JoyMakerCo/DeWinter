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

// TODO: Okay, fine, make commands directly executable.
			AmbitionApp.RegisterCommand<SellItemCmd, ItemVO>(InventoryConsts.SELL_ITEM);
			AmbitionApp.RegisterCommand<BuyItemCmd, ItemVO>(InventoryConsts.BUY_ITEM);
			AmbitionApp.RegisterCommand<DancingCmd, NotableVO>(PartyConstants.START_DANCING);
			AmbitionApp.RegisterCommand<GrantRewardCmd, RewardVO>();
			AmbitionApp.RegisterCommand<CheckMilitaryReputationCmd, AdjustValueVO>();
			AmbitionApp.RegisterCommand<GenerateMapCmd, Party>(MapMessage.GENERATE_MAP);
			AmbitionApp.RegisterCommand<DegradeOutfitCmd, Outfit>(InventoryConsts.BUY_ITEM);
			AmbitionApp.RegisterCommand<IntroServantCmd, string>(ServantConsts.INTRODUCE_SERVANT);
			AmbitionApp.RegisterCommand<HireServantCmd, ServantVO>(ServantConsts.HIRE_SERVANT);
			AmbitionApp.RegisterCommand<FireServantCmd, ServantVO>(ServantConsts.FIRE_SERVANT);
			AmbitionApp.RegisterCommand<LoadSceneCmd, string>(GameMessages.LOAD_SCENE);
			AmbitionApp.RegisterCommand<QuitCmd>(GameMessages.QUIT_GAME);
			AmbitionApp.RegisterCommand<NewGameCmd>(GameMessages.NEW_GAME);
			AmbitionApp.RegisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			AmbitionApp.RegisterCommand<GoToRoomCmd, RoomVO>(MapMessage.GO_TO_ROOM);
			AmbitionApp.RegisterCommand<StartPartyCmd>(PartyMessages.START_PARTY);
			AmbitionApp.RegisterCommand<EndEventCmd, EventVO>(EventMessages.END_EVENT);
			AmbitionApp.RegisterCommand<RSVPCmd, Party>(PartyMessages.RSVP);
			AmbitionApp.RegisterCommand<AdvanceDayCmd>(CalendarMessages.NEXT_DAY);
			AmbitionApp.RegisterCommand<SelectDateCmd, DateTime>(CalendarMessages.SELECT_DATE);

			AmbitionApp.RegisterCommand<PayDayCmd, DateTime>();
			AmbitionApp.RegisterCommand<RestockMerchantCmd, DateTime>();
			AmbitionApp.RegisterCommand<CheckUprisingDayCmd, DateTime>();
			AmbitionApp.RegisterCommand<CreateInvitationsCmd, DateTime>();
			AmbitionApp.RegisterCommand<StartEventCmd, DateTime>();
			AmbitionApp.RegisterCommand<CheckStyleChangeCmd, DateTime>();
			AmbitionApp.RegisterCommand<EventReadyCmd, DateTime>();

			Destroy(this.gameObject);
		}
	}
}