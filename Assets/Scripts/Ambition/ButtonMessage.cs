using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class ButtonMessage : MonoBehaviour
	{
		public string Message;

		protected Button _button;

		void Awake()
		{
			_button = gameObject.GetComponent<Button>();
		}

		void OnEnable()
		{
			_button.onClick.AddListener(OnClick);
		}

		void OnDisable()
		{
			_button.onClick.RemoveListener(OnClick);
		}

		protected virtual void OnClick()
		{
			AmbitionApp.SendMessage(Message);
		}
	}
}
