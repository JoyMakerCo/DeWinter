using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class TimerView : MonoBehaviour
	{
		public float Time;
		public float TotalTime;
		public bool FillUp=false;
		protected event Action<TimerView> Callbacks;

		protected Scrollbar _scrollbar;

		public float Percent
		{
			get { return Time/TotalTime; }
			set { Time = value*TotalTime; }
		}

		public bool Complete
		{
			get { return Time >= TotalTime; }
		}

		void Start()
		{
			_scrollbar = gameObject.GetComponent<Scrollbar>();
		}

		public void Subscribe(Action<TimerView> callback)
		{
			Callbacks += callback;
		}

		public void Unsubscribe(Action<TimerView> callback)
		{
			Callbacks -= callback;
		}

		public void CountDown(float time)
		{
			StopAllCoroutines();
			StartCoroutine(TimerCoroutine(0.0f, time, false));
		}

		public void CountUp(float time)
		{
			StopAllCoroutines();
			StartCoroutine(TimerCoroutine(0.0f, time, true));
		}

		public void Stop()
		{
			StopAllCoroutines();
			if (Callbacks != null) Callbacks(this);
		}

		public void Resume()
		{
			StopAllCoroutines();
			StartCoroutine(TimerCoroutine(Time, TotalTime, FillUp));
		}

		IEnumerator TimerCoroutine(float start, float end, bool fillup)
		{
			FillUp = fillup;
			Time = start;
			TotalTime = end;
			while (Time < TotalTime)
			{
				_scrollbar.value = FillUp ? (Time/TotalTime) : (1.0f - Time/TotalTime);
				Time += UnityEngine.Time.deltaTime;
				yield return null;
			}
			Time = TotalTime;
			_scrollbar.value = FillUp ? 1.0f : 0.0f;
			if (Callbacks != null) Callbacks(this);
		}
	}
}