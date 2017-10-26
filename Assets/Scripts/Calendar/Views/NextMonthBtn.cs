using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class NextMonthBtn : MonoBehaviour
	{
		private Button _btn;
		private DateTime _date;

		void Awake ()
		{
			_btn = GetComponent<Button>();
		}

		void OnEnable()
		{
			CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
			AmbitionApp.Subscribe<DateTime>(CalendarMessages.VIEW_MONTH, HandleMonth);
			_btn.onClick.AddListener(HandleClick);
			_date = calendar.Today;
			_date.AddDays(1-_date.Day);
		}
		
		void OnDisable()
		{
			AmbitionApp.Unsubscribe<DateTime>(CalendarMessages.VIEW_MONTH, HandleMonth);
			_btn.onClick.RemoveListener(HandleClick);
		}

		private void HandleClick()
		{
			AmbitionApp.SendMessage<DateTime>(CalendarMessages.VIEW_MONTH, _date.AddMonths(1));
		}

		private void HandleMonth (DateTime date)
		{
			_date = date;
		}
	}
}
