using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace Ambition
{
	public class TurnTimerView : MonoBehaviour
	{
		private const float GRACE_TIME = .5f;
		
		public GameObject ClockHand;

		private ModelSvc _modelService = App.Service<ModelSvc>();
		private MessageSvc _messageService = App.Service<MessageSvc>();
		private Image _timer;

		void Awake()
		{
			_timer = GetComponent<Image>();
		}

		void OnEnable()
		{
            _messageService.Subscribe(PartyMessages.START_ROUND, HandleRound);
		}

		void OnDisable()
		{
            _messageService.Unsubscribe(PartyMessages.START_ROUND, HandleRound);
		}

		private void HandleRound()
		{
			float t = _modelService.GetModel<PartyModel>().RoundTime;
			StopAllCoroutines();
			StartCoroutine(CountDown(t));
		}

		IEnumerator CountDown (float time)
		{
			float t = 0;
			float ratio;
			while (t < time)
			{
				ratio = t < GRACE_TIME ? 0 : ((t-GRACE_TIME)/(time-GRACE_TIME));
				_timer.fillAmount = ratio;
				t+=Time.deltaTime;
				ClockHand.transform.localRotation = Quaternion.Euler(0f,0f,90f-(360f*ratio));
				yield return null;
			}
			_timer.fillAmount = 0;
			ClockHand.transform.localRotation = Quaternion.Euler(0f,0f,90f);
			_messageService.Send<GuestVO[]>(PartyMessages.GUESTS_SELECTED, null);
		}
	}
}
