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
			AmbitionApp.Subscribe<PartyVO>(PartyMessages.PARTY_UPDATE, HandlePartyUpdated);
			HandleMonth(_today);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<DateTime>(HandleDay);
			AmbitionApp.Unsubscribe<DateTime>(CalendarMessages.VIEW_MONTH, HandleMonth);
			AmbitionApp.Unsubscribe<PartyVO>(HandlePartyUpdated);
			AmbitionApp.Unsubscribe<PartyVO>(PartyMessages.PARTY_UPDATE, HandlePartyUpdated);
		}

		private void HandleDay(DateTime today)
		{
			_today = today;
			UpdateDays();
		}

		private void HandleMonth(DateTime month)
		{
			CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
			List<PartyVO> parties;

			_month = month.AddDays(1-month.Day);
			UpdateDays();

			foreach (CalendarButton day in Days)
			{
				if (model.Parties.TryGetValue(day.Date, out parties))
				{
					parties.ForEach(day.AddParty);
				}
			}
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

		private void HandlePartyUpdated(PartyVO party)
		{
			int index = (party.Date - Days[0].Date).Days;
			if (index < Days.Length) Days[index].AddParty(party);
		}
	}
}
