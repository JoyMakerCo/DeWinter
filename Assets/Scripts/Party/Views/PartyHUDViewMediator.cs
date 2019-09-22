using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class PartyHUDViewMediator : MonoBehaviour
	{
		public Text RoomTitleText;
		public Image reparteeIndicatorImage;
		public GameObject RoomTimer;

		public GameObject DrinkBar;
		public Text DrinkText;

		public GameObject DrinkButton;

		// Player
		public Text PlayerNameText;
		public Image PlayerPortrait;

		private PartyModel _model;

		void Awake()
		{
			//This is used in the Party Scene to brings up the Conversation/Work the Room Window where the Player combats Guests with their charms
			AmbitionApp.Subscribe<int>(PartyMessages.DRINK, HandleDrink);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<int>(PartyMessages.DRINK, HandleDrink);
		}

		void Start ()
		{
			_model = AmbitionApp.GetModel<PartyModel>();
			//RoomTitleText.text = AmbitionApp.GetModel<MapModel>().Room.Value?.Name;
			reparteeIndicatorImage.enabled = (AmbitionApp.GetModel<GameModel>().Level >= 2);
		}

		private void HandleDrink(int drink)
		{
			bool drank = (drink > 0);
			Image image = DrinkButton.GetComponent<Image>();
			image.color =  drank ? Color.white : Color.gray;
			DrinkButton.GetComponent<Collider>().enabled = drank;
		}

		virtual protected void HandleStartTurn()
		{
			// TODO: Clunky as hell. NEED a buff system.
			InventoryModel model = AmbitionApp.GetModel<InventoryModel>();
			float t = AmbitionApp.GetModel<PartyModel>().RoundTime;
            if (model.GetEquippedItem(ItemType.Accessory)?.ID == "Fan")
			{
				t *= 1.1f;
			} 
		}

		IEnumerator Timer(float time)
		{
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
			for (float t = time; t >=0; t-=Time.deltaTime)
			{
				model.Repartee = (t*2 >= time);
				yield return null;
			}
		}

		protected void HandleEndTurn()
		{
			StopAllCoroutines();
		}
	}
}
