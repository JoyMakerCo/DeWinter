using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public abstract class ImageFillMessageView : MonoBehaviour
	{
		public float FillTime = 0.5f;
		public bool AnimateFill=true;
		public bool AnimateEmpty=true;
		private Image _fillbar;		

		void Awake ()
		{
			_fillbar = gameObject.GetComponent<Image>();
		}

		protected void HandlePercent(float percent)
		{
			if (isActiveAndEnabled)
			{
				float currPercent = _fillbar.fillAmount;
				StopAllCoroutines();
				if (percent > currPercent && !AnimateFill)
					_fillbar.fillAmount = percent;
				else if (percent < currPercent && !AnimateEmpty)
					_fillbar.fillAmount = percent;
				else StartCoroutine(InterpValue(percent));
			}
		}

		IEnumerator InterpValue(float value)
		{
			float v0 = _fillbar.fillAmount;
			for (float t = 0; t < FillTime; t+=Time.deltaTime)
			{
				_fillbar.fillAmount = v0 + (value - v0)*(t/FillTime);
				yield return null;
			}
			_fillbar.fillAmount = value;
		}
	}
}
