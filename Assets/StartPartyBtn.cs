using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class StartPartyBtn : MonoBehaviour
	{
		private OutfitInventoryModel _model;
		private Button _button;

		void Start ()
		{
			_model = AmbitionApp.GetModel<OutfitInventoryModel>();
			_button = gameObject.GetComponent<Button>();
			HandleOutfit(_model.Outfit);
			_button.onClick.AddListener(OnClick);
			AmbitionApp.Subscribe<Outfit>(HandleOutfit);
		}

		void OnDisable ()
		{
			AmbitionApp.Unsubscribe<Outfit>(HandleOutfit);
		}

		private void HandleOutfit(Outfit outfit)
		{
			_button.interactable = (outfit != null);
		}

		private void OnClick()
		{
			AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_PARTY);
		}
	}
}
