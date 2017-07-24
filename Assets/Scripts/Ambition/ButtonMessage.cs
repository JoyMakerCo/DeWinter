using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class ButtonMessage : MonoBehaviour
	{
		public string Message;

		protected Button _button;

		void Start()
		{
			_button = gameObject.GetComponent<Button>();
			if (_button != null) _button.onClick.AddListener(OnClick);
		}

		void OnDisable()
		{
			if (_button != null) _button.onClick.RemoveListener(OnClick);
		}

		protected virtual void OnClick()
		{
			AmbitionApp.SendMessage(Message);
		}
	}
}
