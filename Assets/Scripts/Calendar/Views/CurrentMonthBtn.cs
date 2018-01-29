using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class CurrentMonthBtn : MonoBehaviour
	{
		private Button _btn;

		void Awake()
		{
			_btn = GetComponent<Button>();
		}

		void OnEnable()
		{
			_btn.onClick.AddListener(HandleClick);
		}

		void OnDisable()
		{
			_btn.onClick.RemoveListener(HandleClick);
		}

		private void HandleClick()
		{
			DateTime today = AmbitionApp.GetModel<CalendarModel>().Today;
			AmbitionApp.SendMessage<DateTime>(CalendarMessages.VIEW_MONTH, today);
		}
	}
}
