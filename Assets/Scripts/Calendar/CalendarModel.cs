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
		public Dictionary<DateTime, List<PartyVO>> Parties = new Dictionary<DateTime, List<PartyVO>>();

        private DateTime _startDate;
		private int _day = 0;
        private MomentVO _moment;
        public List<IncidentVO> Timeline;
        public List<IncidentVO> Incidents;
        public List<IncidentVO> PastIncidents = new List<IncidentVO>();
        private List<IncidentVO> _queue = new List<IncidentVO>();

		public DateTime StartDate
		{
			get { return _startDate; }
		}

		public string GetDateString()
		{
			return GetDateString(Today);
		}

		public string GetDateString(DateTime d)
		{
			LocalizationModel localization = AmbitionApp.GetModel<LocalizationModel>();
			return d.Day.ToString() + " " + localization.GetList("month")[d.Month-1] + ", " + d.Year.ToString();
		}

		public DateTime Today
		{
			get { return _startDate.AddDays(_day); }
			set {
				_day = (value - _startDate).Days;
				AmbitionApp.SendMessage<DateTime>(value);
			}
		}

        public IncidentVO Incident
        {
            get { return _queue.Count > 0 ? _queue[0] : null; }
            set
            {
                if (value == null) return;
                int index = _queue.LastIndexOf(value);
                if (index == 0 || index == 1) return;
                _queue.Remove(value);
                if (_queue.Count == 0)
                {
                    _queue.Add(value);
                    AmbitionApp.SendMessage<IncidentVO>(value);
                }
                else
                {
                    _queue.Insert(1, value);
                }
            }
        }


		public DateTime EndDate
		{
            get { return Incidents.Last().Date; }
		}

		public DateTime DaysFromNow(int days)
		{
			return _startDate.AddDays(days + _day);
		}

		public DateTime Yesterday
		{
			get { return DaysFromNow(-1); }
		}

        public MomentVO Moment
        {
            get { return _moment; }
            set
            {
                _moment = value;
                AmbitionApp.SendMessage<MomentVO>(_moment);
            }
        }

        public IncidentVO Find(string incidentID)
        {
            IncidentVO incident = Incidents.Find(i => i.Name == incidentID);
            return incident ?? Timeline.Find(i => i.Name == incidentID);
        }

        public void EndIncident()
        {
            if (_queue.Count > 0)
            {
                if (_queue[0].OneShot)
                {
                    PastIncidents.Add(_queue[0]);
                }
                _queue.RemoveAt(0);
            }
            if (_queue.Count > 0)
                AmbitionApp.SendMessage<IncidentVO>(_queue[0]);
        }

		public DateTime NextStyleSwitchDay;

        public void Initialize()
        {
            IncidentConfig[] incidents = Resources.LoadAll<IncidentConfig>("Incidents");
            Timeline = incidents.Where(i=>i.Incident.IsScheduled).Select(i => i.Incident).OrderBy(i=>i.Date).ToList();
            Incidents = incidents.Where(i => !i.Incident.IsScheduled).Select(i => i.Incident).ToList();
            incidents = null;
            _startDate = Timeline[0].Date;
            Resources.UnloadUnusedAssets();
        }
	}
}
