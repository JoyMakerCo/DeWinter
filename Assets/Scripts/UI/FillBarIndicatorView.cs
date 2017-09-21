using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class FillBarIndicatorView : MonoBehaviour
	{
		private const float INTERP_TIME = 0.5f;

		public string ValueID;
		public int Max=100;

		private Image _fillbar;

		void Awake ()
		{
			_fillbar = gameObject.GetComponent<Image>();
			AmbitionApp.Subscribe<int>(ValueID, HandleValue);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<int>(ValueID, HandleValue);
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
				_fillbar.fillAmount += 0.5f*(target-_fillbar.fillAmount);
				yield return null;
			}
			_fillbar.fillAmount = target;
		}
	}
}
