using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using UnityEngine;
using Util;

namespace Ambition
{
    public class CalendarModel : IModel, IInitializable
	{
        public Dictionary<DateTime, List<ICalendarEvent>> Timeline = new Dictionary<DateTime, List<ICalendarEvent>>();
        public List<ICalendarEvent> Unscheduled = new List<ICalendarEvent>();
        public DateTime StartDate;
        public DateTime EndDate;
        public Stack<IncidentVO> IncidentQueue = new Stack<IncidentVO>();

        private int _day;
        private MomentVO _moment;

		public string GetDateString()
		{
			return GetDateString(Today);
		}

        public void Schedule(ICalendarEvent e, DateTime date)
        {
            if (!Timeline.ContainsKey(date))
            {
                Timeline[date] = new List<ICalendarEvent>();
                if (date < StartDate) StartDate = date;
            }
            e.Date = date;
            Unscheduled.Remove(e);
            if (!Timeline[date].Contains(e))
                Timeline[date].Add(e);
            AmbitionApp.SendMessage(e);
        }

        public void Schedule(ICalendarEvent e)
        {
            if (default(DateTime).Equals(e.Date))
            {
                Unscheduled.Add(e);
            }
            else
            {
                if (!Timeline.ContainsKey(e.Date))
                {
                    Timeline[e.Date] = new List<ICalendarEvent>();
                    if (e.Date < StartDate) StartDate = e.Date;
                }
                Timeline[e.Date].Add(e);
            }
        }

        public string GetDateString(DateTime d)
		{
			LocalizationModel localization = AmbitionApp.GetModel<LocalizationModel>();
			return d.Day.ToString() + " " + localization.GetList("month")[d.Month-1] + ", " + d.Year.ToString();
		}

		public DateTime Today
		{
			get { return StartDate.AddDays(_day); }
			set {
				_day = (value - StartDate).Days;
				AmbitionApp.SendMessage(value);
			}
		}

        public IncidentVO Incident
        {
            get { return IncidentQueue.Count > 0 ? IncidentQueue.Peek() : null; }
            set
            {
                if (value != null && !IncidentQueue.Contains(value))
                {
                    IncidentQueue.Push(value);
                    AmbitionApp.SendMessage(value);
                }
            }
        }

		public DateTime DaysFromNow(int days)
		{
			return StartDate.AddDays(days + _day);
		}

		public DateTime Yesterday
		{
			get { return DaysFromNow(-1); }
		}

        public MomentVO Moment
        {
            get { return _moment; }
            set {
                _moment = value;
                if (_moment != null) AmbitionApp.SendMessage(_moment = value);
                else AmbitionApp.SendMessage(IncidentMessages.END_INCIDENT, Incident);
            }
        }

        public IncidentVO FindIncident(string incidentID)
        {
            return Unscheduled.Find(i => i is IncidentVO && ((IncidentVO)i).Name == incidentID) as IncidentVO;
        }

        public PartyVO FindParty(string partyID)
        {
            return Unscheduled.Find(i => i is PartyVO && ((PartyVO)i).Name == partyID) as PartyVO;
        }

        public T[] GetEvents<T>(DateTime date) where T:ICalendarEvent
        {
            List<ICalendarEvent> events;
            return Timeline.TryGetValue(date, out events)
               ? events.OfType<T>().ToArray()
               : new T[0];
        }

        public T[] GetEvents<T>() where T : ICalendarEvent
        {
            return GetEvents<T>(Today);
        }

        public DateTime NextStyleSwitchDay;

        public void Initialize()
        {
            IncidentConfig[] incidents = Resources.LoadAll<IncidentConfig>("Incidents");
            StartDate = DateTime.Today;
            Array.ForEach(incidents, i => Schedule(i.Incident));
            incidents = null;
            Resources.UnloadUnusedAssets();
        }
	}
}
