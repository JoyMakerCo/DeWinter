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
        void Awake() => AmbitionApp.Subscribe<DateTime>(CalendarMessages.VIEW_MONTH, HandleDate);
		void OnDestroy() => AmbitionApp.Unsubscribe<DateTime>(CalendarMessages.VIEW_MONTH, HandleDate);
		private void HandleDate(DateTime t)
		{
            Text text = GetComponent<Text>();
            if (text != null)
			    text.text = AmbitionApp.GetPhrases("month")[t.Month-1];
		}
	}
}
