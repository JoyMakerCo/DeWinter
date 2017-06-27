using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class ScrollbarIndicatorView : MonoBehaviour
	{
		private const float INTERP_TIME = 0.5f;

		public string Type;
		public int Max=100;

		private Scrollbar _scrollbar;

		void Start ()
		{
			_scrollbar = gameObject.GetComponent<Scrollbar>();
			AmbitionApp.Subscribe<int>(Type, HandleValue);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<int>(Type, HandleValue);
		}

		private void HandleValue(int value)
		{
			StopAllCoroutines();
			StartCoroutine(InterpValue(value));
		}

		IEnumerator InterpValue(int value)
		{
			float t0 = Time.time;
			float t1 = Time.time + INTERP_TIME;
			float target = (float)value/(float)Max;
			for (float t = t0; t < t1; t+=Time.deltaTime)
			{
				_scrollbar.value += 0.5f*(target-_scrollbar.value);
				yield return null;
			}
			_scrollbar.value = target;
		}
	}
}