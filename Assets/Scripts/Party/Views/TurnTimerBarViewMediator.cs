using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class TurnTimerBarViewMediator : MonoBehaviour
	{
		private Scrollbar _bar;
		
		void Start()
		{
			AmbitionApp.Subscribe(PartyMessages.START_TIMERS, HandleStartTimer);
			_bar = gameObject.GetComponent<Scrollbar>();
			_bar.value = 1.0f;
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe(PartyMessages.START_TIMERS, HandleStartTimer);
		}

		void HandleStartTimer()
		{
			StartCoroutine(TimerCoroutine(5.0f));
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