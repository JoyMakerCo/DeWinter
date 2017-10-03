using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class StartPartyBtn : MonoBehaviour
	{
		private Button _button;

		void Awake()
		{
			_button = gameObject.GetComponent<Button>();
			AmbitionApp.Subscribe<OutfitVO>(HandleOutfit);
			HandleOutfit(AmbitionApp.GetModel<GameModel>().Outfit);
		}

		void OnEnable ()
		{
			_button.onClick.AddListener(OnClick);
		}

		void OnDisable()
		{
			_button.onClick.RemoveListener(OnClick);
		}

		void OnDestroy ()
		{
			AmbitionApp.Unsubscribe<OutfitVO>(HandleOutfit);
			_button.onClick.RemoveListener(OnClick);
		}

		private void HandleOutfit(OutfitVO outfit)
		{
			_button.interactable = (outfit != null);
		}

		private void OnClick()
		{
			AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_PARTY);
		}
	}
}
