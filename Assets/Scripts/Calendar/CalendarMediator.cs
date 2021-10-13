using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class CalendarMediator : MonoBehaviour
	{
        public CalendarButton[] Days;
        public Text MonthText;
        public Text YearText;
        public GameObject BackBtn;
        public Transform SelectionFrame;

		private int _month;
        private Dictionary<string, string> _monthNames = null;

        public void CurrentMonth() => SetMonth(AmbitionApp.Calendar.Today.Month);
        public void PrevMonth() => SetMonth(_month - 1);
        public void NextMonth() => SetMonth(_month + 1);
        public void UpdateFrame(Transform content)
        {
            SelectionFrame.position = content.position;
            SelectionFrame.gameObject.SetActive(true);
        }

        void OnEnable()
        {
            _month = AmbitionApp.Calendar.Today.Month;
            AmbitionApp.Subscribe<PartyVO>(HandlePartyUpdated);
            AmbitionApp.Calendar.Observe(HandleRefresh);
            AmbitionApp.Subscribe<RendezVO>(HandleRendezvous);
        }


        void OnDisable()
        {
            AmbitionApp.Unsubscribe<PartyVO>(HandlePartyUpdated);
            AmbitionApp.Calendar.Unobserve(HandleRefresh);
            AmbitionApp.Unsubscribe<RendezVO>(HandleRendezvous);
        }

        private void SetMonth(int month)
        {
            _month = month;
            while (_month < 1)
            {
                _month += 12;
            }
            while (_month > 12)
            {
                _month -= 12;
            }
            HandleRefresh(AmbitionApp.Calendar);
        }

        private void HandleRefresh(CalendarModel model)
        {
            CalendarButton btn;
            DateTime today = model.Today;
            DateTime date = new DateTime(today.Year, _month, 1);
            if (!date.Equals(default)) // Immediately after reset
            {
                DateTime startDate = date.AddDays(-(int)(date.DayOfWeek));
                if (_monthNames == null) 
                    _monthNames = AmbitionApp.GetPhrases("month");

                BackBtn.SetActive(_month > model.StartDate.Month);

                _monthNames.TryGetValue(CalendarConsts.MONTH_LOC + (_month - 1), out string monthName);
                MonthText.text = monthName;
                YearText.text = "A.D. " + today.Year.ToString();

                for (int i = Days.Length - 1; i >= 0; i--)
                {
                    btn = Days[i];
                    date = startDate.AddDays(i - 1);
                    btn.SetDay(date, today, _month);
                    UpdateButtonEvents(btn, date);
                }
            }
        }

        private void HandlePartyUpdated(PartyVO party)
		{
            if (party != null)
            {
                DateTime sd = AmbitionApp.Calendar.StartDate;
                CalendarButton btn = Array.Find(Days, b => b.Date.Subtract(sd).Days == party.Day);
                if (btn != null) UpdateButtonEvents(btn, btn.Date);
            }
        }

        private void HandleRendezvous(RendezVO rendez)
        {
            CalendarButton btn = Array.Find(Days, b => b.Date == AmbitionApp.Calendar.StartDate.AddDays(rendez.Day));
            UpdateButtonEvents(btn, btn.Date);
        }

        private void UpdateButtonEvents(CalendarButton btn, DateTime date)
        {
            if (btn != null)
            {
                PartyVO[] parties = AmbitionApp.Calendar.GetOccasions<PartyVO>(date);
                RendezVO[] liaisons = AmbitionApp.Calendar.GetOccasions<RendezVO>(date);
                btn.SetEvents(parties, liaisons);
            }
        }
    }
}
