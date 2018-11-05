using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class CurrentMonthBtn : MonoBehaviour
	{
		public void HandleClick()
		{
			DateTime today = AmbitionApp.GetModel<CalendarModel>().Today;
			AmbitionApp.SendMessage<DateTime>(CalendarMessages.VIEW_MONTH, today);
		}
	}
}
