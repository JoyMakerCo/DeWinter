using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeWinter
{
	public class StartupManager : MonoBehaviour
	{
		public GameObject InventoriesPrefab;

		void Start ()
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

			DeWinterApp.RegisterCommand<SellItemCmd, ItemVO>(InventoryConsts.SELL_ITEM);
			DeWinterApp.RegisterCommand<BuyItemCmd, ItemVO>(InventoryConsts.BUY_ITEM);
			DeWinterApp.RegisterCommand<PayDayCmd, CalendarDayVO>();
			DeWinterApp.RegisterCommand<DancingCmd, NotableVO>(PartyConstants.START_DANCING);
			DeWinterApp.RegisterCommand<GrantRewardCmd, RewardVO>();
			DeWinterApp.RegisterCommand<CheckMilitaryReputationCmd, AdjustValueVO>();
			DeWinterApp.RegisterCommand<RestockMerchantCmd, CalendarDayVO>();
			DeWinterApp.RegisterCommand<GenerateMapCmd, Party>(PartyConstants.GENERATE_MAP);
			DeWinterApp.RegisterCommand<DegradeOutfitCmd, Outfit>(InventoryConsts.BUY_ITEM);
			DeWinterApp.RegisterCommand<IntroServantCmd, string>(ServantConsts.INTRODUCE_SERVANT);
			DeWinterApp.RegisterCommand<HireServantCmd, ServantVO>(ServantConsts.HIRE_SERVANT);
			DeWinterApp.RegisterCommand<FireServantCmd, ServantVO>(ServantConsts.FIRE_SERVANT);

			Instantiate<GameObject>(InventoriesPrefab);

			Destroy(this.gameObject);
		}
	}
}