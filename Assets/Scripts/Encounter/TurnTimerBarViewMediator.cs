using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DeWinter
{
	public class TurnTimerBarViewMediator : MonoBehaviour
	{
		private Scrollbar _bar;
		
		void Start()
		{
			DeWinterApp.Subscribe(PartyMessages.START_TIMERS, HandleStartTimer);
			_bar = gameObject.GetComponent<Scrollbar>();
			_bar.value = 1.0;
		}

		void OnDestroy()
		{
			DeWinterApp.Unsubscribe(PartyMessages.START_TIMERS, HandleStartTimer);
		}

		void HandleStartTimer()
		{
			float timer = 5.0f;
			StartCoroutine(TimerCoroutine());
		}

		IEnumerator TimerCoroutine(float seconds)
		{
			for (float curr = seconds; curr >= 0; curr-=Time.deltaTime)
			{
				_bar.value = curr/seconds;
				yield return null;
			}
			_bar.value = 0;
		}
	}
}