using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Util;

namespace Ambition
{
    [Saveable]
    public class CalendarModel : Model, IResettable, IInitializable, IConsoleEntity
    {
        [JsonIgnore]
        public DateTime StartDate;

        [JsonIgnore]
        public Dictionary<DateTime, List<ICalendarEvent>> Timeline = new Dictionary<DateTime, List<ICalendarEvent>>();

        [JsonIgnore]
        public List<ICalendarEvent> Unscheduled = new List<ICalendarEvent>();

        [JsonProperty("timeline")]
        private List<ICalendarEvent> _events
        {
            set
            {
                Timeline.Clear();
                Unscheduled.Clear();
                foreach(ICalendarEvent e in value)
                {
                    if (e.Date == default) Unscheduled.Add(e);
                    else
                    {
                        if (!Timeline.ContainsKey(e.Date))
                            Timeline.Add(e.Date, new List<ICalendarEvent>());
                        Timeline[e.Date].Add(e);
                    }
                }
            }
            get
            {
                List<ICalendarEvent> events = new List<ICalendarEvent>(Unscheduled);
                foreach (List<ICalendarEvent> scheduled in Timeline.Values)
                    events.AddRange(scheduled);
                return events;
            }
        }

        [JsonProperty("day")]
        private int _day;

        [JsonIgnore]
        public DateTime NextStyleSwitchDay;

        [JsonIgnore]
        public int Day
        {
            get { return _day; }
            set
            {
                _day = value;
                AmbitionApp.SendMessage(Today);
            }
        }

        public void Schedule<T>(T e, DateTime date) where T : ICalendarEvent
        {
            if (e == null) return;
            e.Date = date;
            if (!Timeline.ContainsKey(date)) Timeline[date] = new List<ICalendarEvent>();
            Timeline[date].Add(e);
            Unscheduled.Remove(e);
            AmbitionApp.SendMessage(CalendarMessages.SCHEDULED, e);
            AmbitionApp.SendMessage(e);
        }

        public void Schedule<T>(T e) where T : ICalendarEvent => Schedule(e, (e == default || e.Date == DateTime.MinValue) ? Today : e.Date);

        public bool Delete(ICalendarEvent e)
        {
            bool result = Unscheduled.Remove(e);
            return (Timeline.TryGetValue(e.Date, out List<ICalendarEvent> events) && events.Remove(e)) || result;
        }

        public bool Unschedule(ICalendarEvent e)
        {
            return Timeline.ContainsKey(e.Date) && Timeline[e.Date].Remove(e);
        }

        [JsonIgnore]
        public DateTime Today
        {
            get => StartDate.AddDays(_day);
            set {
                _day = (value - StartDate).Days;
                AmbitionApp.SendMessage(value);
            }
        }

        public DateTime DaysFromNow(int days) => StartDate.AddDays(days + _day);

        [JsonIgnore]
        public DateTime Yesterday => DaysFromNow(-1);

        public T FindUnscheduled<T>(string eventID) where T:ICalendarEvent
        {
            return Unscheduled.OfType<T>().FirstOrDefault(e => e.Name == eventID);
        }

        public T[] FindUnscheduled<T>(Func<T, bool> predicate) => Unscheduled.OfType<T>().Where(predicate).ToArray();

        public ICalendarEvent Find(string EventID) => Unscheduled.FirstOrDefault(e => e.Name == EventID);

        public T[] GetEvents<T>(DateTime date) where T : ICalendarEvent
        {
            if (!Timeline.TryGetValue(date, out List<ICalendarEvent> events)) return new T[0];
            if (date < Today) return events.OfType<T>().ToArray();
            return events.OfType<T>().Where(e => !e.IsComplete).ToArray();
        }

        public T GetEvent<T>(DateTime date) where T : class, ICalendarEvent
        {
            return (Timeline.TryGetValue(date, out List<ICalendarEvent> items))
                ? items.OfType<T>().FirstOrDefault()
                : null;
        }

        public T GetEvent<T>() where T : class, ICalendarEvent => GetEvent<T>(Today);
        public T[] GetEvents<T>() where T : class, ICalendarEvent => GetEvents<T>(Today);
        public T[] GetUnscheduledEvents<T>() where T : ICalendarEvent => Unscheduled.OfType<T>().ToArray();

        public void Initialize()
        {
            IncidentConfig[] incidents = Resources.LoadAll<IncidentConfig>("Incidents");
            StartDate = DateTime.Today;
            IncidentVO incident;
            foreach (IncidentConfig config in incidents)
            {
                incident = config.GetIncident();
                if (incident.IsScheduled)
                {
                    Schedule(incident);
                    if (incident.Date < StartDate)
                    {
                        StartDate = incident.Date;
                    }
                }
                else Unscheduled.Add(incident);
            }
            incidents = null;
            Resources.UnloadUnusedAssets();
        }

        public void Complete<T>(T e) where T : ICalendarEvent
        {
            if (!IsComplete(e))
            {
                e.IsComplete = true;
                Unscheduled.Remove(e);
                AmbitionApp.SendMessage(CalendarMessages.CALENDAR_EVENT_COMPLETED, e);
            }
        }

        public bool IsComplete(ICalendarEvent e) => e == null || e.IsComplete || e.Date < Today;

        public void Reset()
        {
            Timeline.Clear();
            _day = 0;
            StartDate = default;
        }

        public string[] Dump()
        {
            var dateFormat = "MMMM d, yyyy";

            var lines = new List<string>()
            {
                "CalendarModel:",
                string.Format( "Today {0} (day {1})", Today.ToString(dateFormat), Day ),
                "Started: " + StartDate.ToString(dateFormat),
                "Next Style Switch: " + NextStyleSwitchDay.ToString(dateFormat),
            };

            lines.Add( "Unscheduled Events: " + Unscheduled.Count.ToString() );
            foreach (var ev in Unscheduled)
            {
                lines.Add( string.Format( "  {0}", ev.Name ));
            }
        
            lines.Add( "Timeline Events: " + Timeline.Count.ToString() );
            foreach (var eventList in Timeline.OrderBy(kv => kv.Key).Select(kv => kv.Value))
            {
                foreach (var ev in eventList)
                {
                    lines.Add( string.Format( "  {0}: {1}", ev.Date.ToString(dateFormat), ev.Name ));
                }
            }

            return lines.ToArray();
        }

        public void Invoke( string[] args )
        {
            ConsoleModel.warn("CalendarModel has no invocation.");
        }    
    }
}
