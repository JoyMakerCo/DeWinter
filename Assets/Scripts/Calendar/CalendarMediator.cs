using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class CalendarMediator : MonoBehaviour
	{
		public CalendarButton[] Days;

		private DateTime _month;
		private DateTime _today;
	
		void Awake ()
		{
			StartCoroutine(WaitToStart());
		}

		IEnumerator WaitToStart()
		{
			while (!Array.TrueForAll(Days, d=>d.isActiveAndEnabled))
				yield return null;
				
			_today = AmbitionApp.GetModel<CalendarModel>().Today;
			AmbitionApp.Subscribe<DateTime>(HandleDay);
			AmbitionApp.Subscribe<DateTime>(CalendarMessages.VIEW_MONTH, HandleMonth);
			AmbitionApp.Subscribe<PartyVO>(HandlePartyUpdated);
			AmbitionApp.Subscribe<PartyVO>(PartyMessages.RSVP, HandlePartyUpdated);
			HandleMonth(_today);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<DateTime>(HandleDay);
			AmbitionApp.Unsubscribe<DateTime>(CalendarMessages.VIEW_MONTH, HandleMonth);
			AmbitionApp.Unsubscribe<PartyVO>(HandlePartyUpdated);
			AmbitionApp.Unsubscribe<PartyVO>(PartyMessages.RSVP, HandlePartyUpdated);
		}

		private void HandleDay(DateTime today)
		{
			_today = today;
			UpdateDays();
		}

		private void HandleMonth(DateTime month)
		{
			_month = month.AddDays(1-month.Day);
			UpdateDays();
			UpdateParties();
		}

		private void UpdateDays()
		{
			DateTime startDate = _month.AddDays(-(int)(_month.DayOfWeek));
			int max = Days.Length;
			for (int i=0; i<max; i++)
			{
				Days[i].SetDay(startDate.AddDays(i), _today, _month);
			}
		}

		private void UpdateParties()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			PartyVO [] parties = Array.FindAll(model.Parties, p=>p.Date >= Days[0].Day && p.Date <= Days[Days.Length-1].Day);
			Array.ForEach(parties, HandlePartyUpdated);
		}

		private void HandlePartyUpdated(PartyVO party)
		{
			CalendarButton btn = Array.Find(Days, d=>d.Day == party.Date);
			if (btn != null) btn.AddParty(party);
		}
	}
}
