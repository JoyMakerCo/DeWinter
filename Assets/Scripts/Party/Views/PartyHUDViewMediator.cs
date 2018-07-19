using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class PartyHUDViewMediator : MonoBehaviour
	{
		public Text RoomTitleText;
		public Image reparteeIndicatorImage;
		public GameObject RoomTimer;

		public Scrollbar ConfidenceBar;
		public Text ConfidenceText;

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
			AmbitionApp.Subscribe<int>(GameConsts.CONFIDENCE, HandleConfidence);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<int>(GameConsts.CONFIDENCE, HandleConfidence);
			AmbitionApp.Unsubscribe<int>(PartyMessages.DRINK, HandleDrink);
		}

		void Start ()
		{
			_model = AmbitionApp.GetModel<PartyModel>();
			RoomTitleText.text = AmbitionApp.GetModel<MapModel>().Room.Name;
			reparteeIndicatorImage.enabled = (AmbitionApp.GetModel<GameModel>().Level >= 2);
		}

		private void HandleConfidence(int confidence)
		{
			ConfidenceText.text = "Confidence: " + confidence.ToString() + " / " + _model.MaxConfidence.ToString();
			ConfidenceText.color = (confidence <= 25) ? Color.red : Color.white;
			ConfidenceBar.value = (float)confidence/(float)_model.MaxConfidence;
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
			ItemVO accessory;
			float t = AmbitionApp.GetModel<PartyModel>().RoundTime;
			if (model.Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory) && accessory.Name == "Fan")
			{
				t *= 1.1f;
			} 
			// TODO: Reset & Start timer
//			RoomTimer.CountDown(t);
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
