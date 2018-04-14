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
			AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeOut);
			_button.onClick.RemoveListener(OnClick);
		}

		private void HandleOutfit(OutfitVO outfit)
		{
			_button.interactable = (outfit != null);
		}

		private void OnClick()
		{
			_button.interactable = false;
			AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeOut);
			AmbitionApp.SendMessage(GameMessages.FADE_OUT);
		}

		private void HandleFadeOut()
		{
			AmbitionApp.SendMessage<PartyVO>(MapMessage.GENERATE_MAP, AmbitionApp.GetModel<PartyModel>().Party);
			AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.PARTY_SCENE);
		}
	}
}
