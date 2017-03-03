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
			DeWinterApp.RegisterModel<InventoryModel>();
			DeWinterApp.RegisterModel<ServantModel>();
			DeWinterApp.RegisterModel<PartyModel>();
			DeWinterApp.RegisterModel<CalendarModel>();
			DeWinterApp.RegisterModel<DevotionModel>();
			DeWinterApp.RegisterModel<EventModel>();
			DeWinterApp.RegisterModel<QuestModel>();

			Instantiate<GameObject>(InventoriesPrefab);

			Destroy(this.gameObject);
		}
	}
}