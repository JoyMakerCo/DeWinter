using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using Newtonsoft.Json;
using Util;

namespace Ambition
{
    public interface ICalendar { void Clear(); }

    // Convenience model, fed by other models
    [Saveable]
    public class CalendarModel : ObservableModel<CalendarModel>, IResettable
    {
        // PUBLIC DATA //////////////////

        [JsonIgnore] // Set by Player Data
        public DateTime StartDate;

        [JsonIgnore]
        public DateTime Today
        {
            get => StartDate.AddDays(Day);
            set => Day = value.Subtract(StartDate).Days;
        }

        [JsonProperty("day")] // Literally the only thing being serialized
        public int Day = 0;

        // PRIVATE DATA //////////////////

        [JsonIgnore]
        private Dictionary<Type, ICalendar> _calendars = new Dictionary<Type, ICalendar>();

        // PUBLIC METHODS //////////////////

        public void RegisterCalendar<T>(Calendar<T> calendar)
        {
            Type t = typeof(T);
            _calendars[t] = calendar;
            calendar._model = this;
        }

        public void Schedule<T>(T occasion, DateTime date) => Schedule(occasion, date.Subtract(StartDate).Days);
        public void Schedule<T>(T occasion, int day)
        {
            _calendars.TryGetValue(typeof(T), out ICalendar cal);
            (cal as Calendar<T>)?.Schedule(occasion, day);
            Broadcast();
        }

        public T[] GetOccasions<T>()
        {
            _calendars.TryGetValue(typeof(T), out ICalendar cal);
            return (cal as Calendar<T>)?.GetOccasions(Day) ?? new T[0];
        }

        public T[] GetOccasions<T>(DateTime date)
        {
            _calendars.TryGetValue(typeof(T), out ICalendar cal);
            return (cal as Calendar<T>)?.GetOccasions(date.Subtract(StartDate).Days) ?? new T[0];
        }

        public T[] GetOccasions<T>(int day)
        {
            _calendars.TryGetValue(typeof(T), out ICalendar cal);
            return (cal as Calendar<T>)?.GetOccasions(day) ?? new T[0];
        }

        public T[] GetOccasions<T>(DateTime startDate, DateTime endDate)
        {
            int startDay = startDate.Subtract(StartDate).Days;
            int endDay = endDate.Subtract(StartDate).Days;
            return GetOccasions<T>(startDay, endDay);
        }

        public T[] GetOccasions<T>(int startDay, int endDay)
        {
            Calendar<T> calendar;
            _calendars.TryGetValue(typeof(T), out ICalendar cal);
            calendar = cal as Calendar<T>;
            if (calendar == null) return new T[0];

            List<T> result = new List<T>();
            if (startDay > endDay)
            {
                int day = endDay;
                endDay = startDay;
                startDay = day;
            }
            for (int day = startDay; day <= endDay; ++day)
            {
                result.AddRange(calendar.GetOccasions(day));
            }
            return result.ToArray();
        }


        public bool Remove<T>(T calendarEvent) where T:CalendarEvent
        {
            if (calendarEvent == null || !_calendars.TryGetValue(typeof(T), out ICalendar ic)) return false;
            Calendar<T> cal = ic as Calendar<T>;
            return cal != null
                && cal.TryGetValue(calendarEvent.Day, out List<T> events)
                && (events?.Remove(calendarEvent) ?? false);
        }

        public bool HasNewEvents<T>(bool futureEvents, bool pastEvents) where T : CalendarEvent
        {
            List<T> result = new List<T>();
            Calendar<T> calendar = GetCalendar<T>();
            if (calendar == null) return false;

            foreach (KeyValuePair<int, List<T>> kvp in calendar)
            {
                if (kvp.Key == calendar.Day
                    || (futureEvents && kvp.Key > calendar.Day)
                    || (pastEvents && kvp.Key < calendar.Day))
                {
                    if (kvp.Value.Exists(t => t.Created < 0 && t.RSVP == RSVP.New))
                        return true;
                }
            }
            return false;
        }

        public List<T> GetNewEvents<T>(bool futureEvents, bool pastEvents) where T : CalendarEvent
        {
            List<T> result = new List<T>();
            Calendar<T> calendar = GetCalendar<T>();
            if (calendar == null) return result;

            List<T> search;
            foreach (KeyValuePair<int, List<T>> kvp in calendar)
            {
                if (kvp.Key == calendar.Day
                    || (futureEvents && kvp.Key > calendar.Day)
                    || (pastEvents && kvp.Key < calendar.Day))
                {
                    search = kvp.Value.FindAll(t => t.Created < 0 && t.RSVP == RSVP.New);
                    result.AddRange(search);
                }
            }
            return result;
        }


        public void Reset()
        {
            Day = 0;
            StartDate = default;
            _calendars.Clear();
            Broadcast();
        }

        public override string ToString()
        {
            string dateFormat = "MMMM d, yyyy";
            return "CalendarModel:"
                + string.Format("Today {0} (day {1})", Today.ToString(dateFormat), Day)
                + "Started: " + StartDate.ToString(dateFormat);
        }

        // PRIVATE METHODS //////////////////

        private Calendar<T> GetCalendar<T>()
        {
            _calendars.TryGetValue(typeof(T), out ICalendar ical);
            return ical as Calendar<T>;
        }
    }

    [Serializable]
    public abstract class Calendar<T> : Dictionary<int, List<T>>, ICalendar
    {
        [JsonIgnore]
        internal CalendarModel _model;

        [JsonIgnore]
        public DateTime Today => _model?.Today ?? default;

        [JsonIgnore]
        public int Day => _model?.Day ?? -1;

        [JsonIgnore]
        public DateTime StartDate => _model?.StartDate ?? default;

        public T[] GetOccasions(Predicate<T> criteria)
        {
            List<T> result = new List<T>();
            foreach(List<T> list in Values)
            {
                result.AddRange(list.FindAll(criteria));
            }
            return result.ToArray();
        }

        public T[] GetOccasions(DateTime date) => GetOccasions(date.Subtract(StartDate).Days);
        public T[] GetOccasions(int day)
        {
            return TryGetValue(day, out List<T> events)
                ? events?.ToArray()
                : new T[0];
        }

        public void Schedule(T t, DateTime date) => Schedule(t, date.Subtract(StartDate).Days);
        public void Schedule(T t, int day)
        {
            if (!this.ContainsKey(day) || this[day] == null)
                this[day] = new List<T>() { t };
            else this[day].Add(t);
        }
    }
}
