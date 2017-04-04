using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeWinter
{
	public class StartupManager : MonoBehaviour
	{
		public GameObject InventoriesPrefab;

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

// TODO: Okay, fine, make commands directly executable.
			DeWinterApp.RegisterCommand<SellItemCmd, ItemVO>(InventoryConsts.SELL_ITEM);
			DeWinterApp.RegisterCommand<BuyItemCmd, ItemVO>(InventoryConsts.BUY_ITEM);
			DeWinterApp.RegisterCommand<PayDayCmd, CalendarDayVO>();
			DeWinterApp.RegisterCommand<RestockMerchantCmd, CalendarDayVO>();
			DeWinterApp.RegisterCommand<CheckUprisingDayCmd, CalendarDayVO>();
			DeWinterApp.RegisterCommand<DancingCmd, NotableVO>(PartyConstants.START_DANCING);
			DeWinterApp.RegisterCommand<GrantRewardCmd, RewardVO>();
			DeWinterApp.RegisterCommand<CheckMilitaryReputationCmd, AdjustValueVO>();
			DeWinterApp.RegisterCommand<GenerateMapCmd, Party>(MapMessage.GENERATE_MAP);
			DeWinterApp.RegisterCommand<DegradeOutfitCmd, Outfit>(InventoryConsts.BUY_ITEM);
			DeWinterApp.RegisterCommand<IntroServantCmd, string>(ServantConsts.INTRODUCE_SERVANT);
			DeWinterApp.RegisterCommand<HireServantCmd, ServantVO>(ServantConsts.HIRE_SERVANT);
			DeWinterApp.RegisterCommand<FireServantCmd, ServantVO>(ServantConsts.FIRE_SERVANT);
			DeWinterApp.RegisterCommand<PreparePartyCmd>(PartyMessages.PREPARE_PARTY);
			DeWinterApp.RegisterCommand<LoadSceneCmd, string>(GameMessages.LOAD_SCENE);
			DeWinterApp.RegisterCommand<QuitCmd>(GameMessages.QUIT_GAME);
			DeWinterApp.RegisterCommand<StartEventCmd, CalendarDayVO>();
			DeWinterApp.RegisterCommand<EventReadyCmd, string>(GameMessages.SCENE_READY);
			DeWinterApp.RegisterCommand<NewGameCmd>(GameMessages.NEW_GAME);
			DeWinterApp.RegisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			DeWinterApp.RegisterCommand<GoToRoomCmd, RoomVO>(MapMessage.GO_TO_ROOM);
			DeWinterApp.RegisterCommand<StartPartyCmd>(PartyMessages.START_PARTY);


// TODO: Get rid of this when the Calendar Inventory is swallowed buy the model
Instantiate<GameObject>(InventoriesPrefab);

			Destroy(this.gameObject);
		}
	}
}