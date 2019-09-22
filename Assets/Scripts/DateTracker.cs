using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Globalization;

namespace Ambition
{
	public class DateTracker : MonoBehaviour
	{
	    private Text _text;
	    private CalendarModel _calendar;

	    void Awake()
	    {
			_text = GetComponent<Text>();
			_calendar = AmbitionApp.GetModel<CalendarModel>();
			AmbitionApp.Subscribe<DateTime>(HandleDate);
			HandleDate(_calendar.Today);
	    }

	    void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<DateTime>(HandleDate);
	    }

		public void HandleDate(DateTime date)
	    {
            _text.text = AmbitionApp.GetModel<LocalizationModel>().Date;
        }
	}
}
