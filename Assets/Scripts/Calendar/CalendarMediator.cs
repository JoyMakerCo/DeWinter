using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class CalendarMediator : MonoBehaviour
	{
		public CalendarButton[] Days;

		private int _month;
		private CalendarModel _model;

        private void Awake() => _model = AmbitionApp.GetModel<CalendarModel>();

        private DateTime Today => _model.Today;

        void Start()
		{
            AmbitionApp.Subscribe(CalendarMessages.PREV_MONTH, HandlePrevMonth);
            AmbitionApp.Subscribe(CalendarMessages.NEXT_MONTH, HandleNextMonth);
            AmbitionApp.Subscribe(CalendarMessages.CURRENT_MONTH, HandleCurrentMonth);
			AmbitionApp.Subscribe<PartyVO>(HandlePartyUpdated);
			ViewMonth(Today.Month);
		}

		void OnDestroy()
		{
            AmbitionApp.Unsubscribe(CalendarMessages.PREV_MONTH, HandlePrevMonth);
            AmbitionApp.Unsubscribe(CalendarMessages.NEXT_MONTH, HandleNextMonth);
            AmbitionApp.Unsubscribe(CalendarMessages.CURRENT_MONTH, HandleCurrentMonth);
            AmbitionApp.Unsubscribe<PartyVO>(HandlePartyUpdated);
		}

        private void HandleCurrentMonth()
        {
            ViewMonth(Today.Month);
        }

        private void HandlePrevMonth()
        {
            if (_month > 1)
                ViewMonth(_month - 1);
        }

        private void HandleNextMonth()
        {
            if (_month < 7)
                ViewMonth(_month + 1);
        }


        private void ViewMonth(int month)
		{
            CalendarButton btn;
            _month = month;
            DateTime date = new DateTime(Today.Year, _month, 1);
            DateTime startDate = date.AddDays(-(int)(date.DayOfWeek));

			for (int i = Days.Length-1; i>=0; i--)
			{
                btn = Days[i];
				btn.SetDay(startDate.AddDays(i-1), Today, _month);
                btn.SetParties(GetParties(btn.Date));
            }
            AmbitionApp.SendMessage(CalendarMessages.VIEW_MONTH, date);
        }

        private void HandlePartyUpdated(PartyVO party)
		{
            if (party != null)
            {
                CalendarButton btn = Array.Find(Days, d => d.Date == party.Date);
                btn?.SetParties(GetParties(party.Date));
            }
        }

        private PartyVO[] GetParties(DateTime date)
        {
            int day = date.Subtract(AmbitionApp.GetModel<CalendarModel>().StartDate).Days;
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            OccasionVO[] occasions = _model.GetOccasions(OccasionType.Party, day);
            PartyVO[] parties = new PartyVO[occasions.Length];
            PartyVO party;
            for (int i=occasions.Length-1; i>=0; --i)
            {
                model.Parties.TryGetValue(occasions[i].ID, out party);
                parties[i] = party;
            }
            return parties;
        }
    }
}
