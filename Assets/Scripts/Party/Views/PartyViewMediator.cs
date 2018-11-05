using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class PartyViewMediator : MonoBehaviour
	{
        private const float FADE_TIME = 1.5f;

		public GameObject MapView;
		public GameObject RoomView;
        public PartySoundMediator SoundMediator;
        public Image Fader;
        public Text TitleText;
        GameObject _roomView;

        // Use this for initialization
        void Awake()
		{
            AmbitionApp.Subscribe(PartyMessages.SHOW_ROOM, GoToRoom);
            AmbitionApp.Subscribe(PartyMessages.SHOW_MAP, GoToMap);
            SetTitle();
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe(PartyMessages.SHOW_ROOM, GoToRoom);
			AmbitionApp.Unsubscribe(PartyMessages.SHOW_MAP, GoToMap);
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, OnFadeOut);
        }

        private void SetTitle()
        {
            PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
            TitleText.text = partyModel.Party.Name;
        }

		private void GoToRoom()
		{
            StopAllCoroutines();
            Fader.enabled = false;
            AmbitionApp.SendMessage(GameMessages.FADE_OUT);
            AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, OnFadeOut);
		}

        private void OnFadeOut()
        {
            if (_roomView == null)
                _roomView = Instantiate(RoomView, this.transform);
            _roomView.SetActive(true);
            Fader.transform.SetAsLastSibling();
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, OnFadeOut);
            AmbitionApp.SendMessage(GameMessages.FADE_IN);
        }

        private void GoToMap()
		{
            StopAllCoroutines();
            if (_roomView != null && _roomView.activeSelf)
                StartCoroutine(FadeToMap());
		}

        // TODO: Per user feedback, this is going to be replaced by a standard fade
        IEnumerator FadeToMap()
        {
            float fadeTime1 = 1f / FADE_TIME;
            Fader.transform.SetAsLastSibling();
            Fader.enabled = true;
            Color color = Fader.color;
            for (float t = FADE_TIME; t > 0; t -= Time.deltaTime)
            {
                color.a = (FADE_TIME - t) * fadeTime1;
                Fader.color = color;
                yield return null;
            }
            color.a = 1f;
            Fader.color = color;
            if (_roomView != null) _roomView.SetActive(false);
            MapView.SetActive(true);
            AmbitionApp.SendMessage(GameMessages.FADE_OUT_COMPLETE);

            Fader.transform.SetAsLastSibling();
            for (float t = FADE_TIME; t > 0; t -= Time.deltaTime)
            {
                color.a = t * fadeTime1;
                Fader.color = color;
                yield return null;
            }
            AmbitionApp.SendMessage(GameMessages.FADE_IN_COMPLETE);
            Fader.enabled = false;
        }
    }
}
