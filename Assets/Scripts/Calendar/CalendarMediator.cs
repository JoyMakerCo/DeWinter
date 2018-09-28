using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class CalendarMediator : MonoBehaviour
	{
		public CalendarButton[] Days;

		private DateTime _month;
		private CalendarModel _model;

        private void Awake()
        {
            _model = AmbitionApp.GetModel<CalendarModel>();
        }

        private DateTime Today => _model.Today;

        void Start()
		{
			AmbitionApp.Subscribe<DateTime>(CalendarMessages.VIEW_MONTH, HandleMonth);
			AmbitionApp.Subscribe<PartyVO>(HandlePartyUpdated);
			HandleMonth(Today);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<DateTime>(CalendarMessages.VIEW_MONTH, HandleMonth);
			AmbitionApp.Unsubscribe<PartyVO>(HandlePartyUpdated);
		}

		private void HandleMonth(DateTime month)
		{
			CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
            List<ICalendarEvent> events;

			_month = month.AddDays(1-month.Day);
			UpdateDays();

			foreach (CalendarButton day in Days)
			{
                if (model.Timeline.TryGetValue(day.Date, out events))
				{
                    foreach (ICalendarEvent e in events)
                        day.AddParty(e as PartyVO);
				}
			}
		}

		private void UpdateDays()
		{
			DateTime startDate = _month.AddDays(-(int)(_month.DayOfWeek));
			for (int i = Days.Length-1; i>=0; i--)
			{
				Days[i].SetDay(startDate.AddDays(i), Today, _month);
			}
		}

		private void HandlePartyUpdated(PartyVO party)
		{
            if (party != null)
            {
                int index = (party.Date - Days[0].Date).Days;
                if (index >= 0 && index < Days.Length) Days[index].AddParty(party);
            }
        }
	}
}
