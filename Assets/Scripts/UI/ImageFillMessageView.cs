using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public abstract class ImageFillMessageView<T> : MonoBehaviour
	{
		[SerializeField]
		protected string _valueID;

		protected float _interpolationTime = 0.5f;

		private Image _fillbar;

		public string ValueID
		{
			get { return _valueID; }
			set {
				AmbitionApp.Unsubscribe<T>(_valueID, HandleValue);
				AmbitionApp.Subscribe<T>(_valueID=value, HandleValue);
			}
		}

		protected T _value;
		public T Value
		{
			get { return _value; }
			set
			{
				float percent = CalculatePercent(_value = value);
				if (isActiveAndEnabled)
					StartCoroutine(InterpValue(percent));
				else
					_fillbar.fillAmount = percent;
			}
		}

		public float Percent
		{
			get { return _fillbar.fillAmount; }
			set { _fillbar.fillAmount = value; }
		}

		void Awake ()
		{
			ValueID = _valueID;
			_fillbar = gameObject.GetComponent<Image>();
		}

		void OnEnable()
		{
			AmbitionApp.Subscribe<T>(_valueID, HandleValue);
		}

		void OnDisable()
		{
			StopAllCoroutines();
			AmbitionApp.Unsubscribe<T>(_valueID, HandleValue);
		}

		protected abstract float CalculatePercent(T value);

		private void HandleValue(T value)
		{
			if (isActiveAndEnabled)
			{
				StopAllCoroutines();
				Value = value;
			}
		}

		IEnumerator InterpValue(float value)
		{
			float v0 = _fillbar.fillAmount;
			for (float t = 0; t < _interpolationTime; t+=Time.deltaTime)
			{
				_fillbar.fillAmount = ((_interpolationTime - t)*v0 + t*value)/_interpolationTime;
				yield return null;
			}
			_fillbar.fillAmount = value;
		}
	}
}
