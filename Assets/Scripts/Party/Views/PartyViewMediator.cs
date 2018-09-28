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
        GameObject _roomView;

        // Use this for initialization
        void Awake()
		{
            AmbitionApp.Subscribe(PartyMessages.SHOW_ROOM, GoToRoom);
            AmbitionApp.Subscribe(PartyMessages.SHOW_MAP, GoToMap);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe(PartyMessages.SHOW_ROOM, GoToRoom);
			AmbitionApp.Unsubscribe(PartyMessages.SHOW_MAP, GoToMap);
		}

        void Start ()   
		{
            AmbitionApp.SendMessage(GameMessages.FADE_IN);
		}

		private void GoToRoom()
		{
            StopAllCoroutines();
            StartCoroutine(ChooseView(true));
		}

		private void GoToMap()
		{
            StopAllCoroutines();
            StartCoroutine(ChooseView(false));
		}

        IEnumerator ChooseView(bool showRoom)
        {
            if (showRoom == MapView.activeSelf)
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
                MapView.SetActive(!showRoom);
                if (showRoom)
                {
                    if (_roomView == null)
                        _roomView = Instantiate(RoomView, this.transform);
                    Fader.transform.SetAsLastSibling();
                    AmbitionApp.InvokeMachine("ConversationController");
                }
                else Destroy(_roomView);
                for (float t = FADE_TIME; t > 0; t -= Time.deltaTime)
                {
                    color.a = t * fadeTime1;
                    Fader.color = color;
                    yield return null;
                }
                Fader.enabled = false;
            }
        }
    }
}
