using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public abstract class SliderFillMessageView : MonoBehaviour
	{
		public float FillTime = 0.5f;
		public bool AnimateFill=true;
		public bool AnimateEmpty=true;
		protected Slider _slider;		

		void Awake ()
		{
			_slider = gameObject.GetComponent<Slider>();
		}

		protected void HandleValue(float value)
		{
			if (isActiveAndEnabled)
			{
				StopAllCoroutines();
				if (value > _slider.value && !AnimateFill)
					_slider.value = value;
				else if (value < _slider.value && !AnimateEmpty)
					_slider.value = value;
				else StartCoroutine(InterpValue(value));
			}
		}

		IEnumerator InterpValue(float value)
		{
			float v0 = _slider.value;
			for (float t = 0; t < FillTime; t+=Time.deltaTime)
			{
				_slider.value = v0 + (value - v0)*(t/FillTime);
				yield return null;
			}
			_slider.value = value;
		}
	}
}
