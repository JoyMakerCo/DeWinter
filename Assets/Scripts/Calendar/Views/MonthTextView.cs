using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace Ambition
{
	public class MonthTextView : MonoBehaviour
	{
		private Text _text;

		void Awake()
		{
			_text = GetComponent<Text>();
			HandleDate(AmbitionApp.GetModel<CalendarModel>().Today);
		}

		void OnEnable()
		{
			AmbitionApp.Subscribe<DateTime>(CalendarMessages.VIEW_MONTH, HandleDate);
		}

		void OnDisable()
		{
			AmbitionApp.Unsubscribe<DateTime>(HandleDate);
		}

		private void HandleDate(DateTime t)
		{
			_text.text = AmbitionApp.GetModel<LocalizationModel>().GetList("month")[t.Month-1];
		}
	}
}
