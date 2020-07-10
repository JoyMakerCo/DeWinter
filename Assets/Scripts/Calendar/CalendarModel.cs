using Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Util;

namespace Ambition
{
    [Saveable]
    public class CalendarModel : Model, IResettable, IConsoleEntity
    {
        // PUBLIC DATA //////////////////

        [JsonIgnore]
        public DateTime StartDate;

        [JsonProperty("day")]
        public int Day;

        [JsonIgnore]
        public DateTime Today
        {
            get => StartDate.AddDays(Day);
            set => Day = value.Subtract(StartDate).Days;
        }

        // PRIVATE DATA //////////////////

        [JsonProperty("timeline")]
        private Dictionary<int, List<OccasionVO>> _timeline = new Dictionary<int, List<OccasionVO>>();

        // PUBLIC METHODS //////////////////

        public void Schedule(string id, OccasionType type) => Schedule(new OccasionVO()
        {
            ID = id,
            Day = Day,
            Type = type,
            IsComplete = false
        });

        public void Schedule(string id, int day, OccasionType type, bool isComplete) => Schedule(new OccasionVO()
        {
            ID = id,
            Day = day,
            Type = type,
            IsComplete = isComplete
        });

        public void Schedule(string id, int day, OccasionType type) => Schedule(new OccasionVO()
        {
            ID = id,
            Day = day,
            Type = type,
            IsComplete = day < Day
        });

        public void Schedule(OccasionVO occasion)
        {
            if (!_timeline.TryGetValue(occasion.Day, out List<OccasionVO> occasions))
            {
                _timeline.Add(occasion.Day, occasions = new List<OccasionVO>());
            }
            if (occasions.FindIndex(o=>o.ID == occasion.ID) < 0)
            {
                occasions.Add(occasion);
            }
            AmbitionApp.SendMessage(CalendarMessages.SCHEDULED, occasion);
            AmbitionApp.SendMessage(occasion);
            Broadcast();
        }

        public OccasionVO[] GetOccasions(OccasionType type, bool includeComplete=false) => GetOccasions(type, Day, includeComplete);
        public OccasionVO[] GetOccasions(OccasionType type, int day, bool includeComplete=false)
        {
            if (!_timeline.TryGetValue(day, out List<OccasionVO> occasions)) return new OccasionVO[0];
            List<OccasionVO> result = new List<OccasionVO>();
            foreach(OccasionVO occasion in occasions)
            {
                if (occasion.Type == type && (includeComplete || !occasion.IsComplete))
                    result.Add(occasion);
            }
            return result.ToArray();
        }

        public OccasionVO GetOccasion(OccasionType type) => GetOccasion(type, Day, false);
        public OccasionVO GetOccasion(OccasionType type, bool includeComplete) => GetOccasion(type, Day, includeComplete);
        public OccasionVO GetOccasion(OccasionType type, int day) => GetOccasion(type, day, false);
        public OccasionVO GetOccasion(OccasionType type, int day, bool includeComplete)
        {
            if (_timeline.TryGetValue(day, out List<OccasionVO> occasions))
            {
                int index = occasions.FindIndex(o => o.Type == type && (includeComplete || !o.IsComplete));
                if (index >= 0) return occasions[index];
            }
            return default;
        }

        public void Complete(string id, OccasionType type)
        {
            Debug.Log("CalendarModel completing event");
            OccasionVO occasion;
            List<OccasionVO> occasions;
            int index = _timeline.TryGetValue(Day, out occasions)
                ? occasions.FindIndex(o => o.ID == id && o.Type == type)
                : -1;
            if (index < 0)
            {
                foreach(List<OccasionVO> date in _timeline.Values)
                {
                    index = date.FindIndex(o => o.ID == id && o.Type == type);
                    if (index >= 0) break;
                }
            }
            if (index >= 0)
            {
                occasion = occasions[index];
                occasion.IsComplete = true;
                occasions[index] = occasion;
                AmbitionApp.SendMessage(CalendarMessages.OCCASION_COMPLETED, occasion);
                Broadcast();
            }
        }

        public void Reset()
        {
            _timeline.Clear();
            Day = 0;
            StartDate = default;
            Broadcast();
        }

        public bool Remove(string id, OccasionType type)
        {
            foreach(List<OccasionVO> occasions in _timeline.Values)
            {
                if (occasions.RemoveAll(o=>o.ID == id && o.Type == type) > 0)
                {
                    Broadcast();
                    return true;
                }
            }
            return false;
        }

        public string[] Dump()
        {
            var dateFormat = "MMMM d, yyyy";

            var lines = new List<string>()
            {
                "CalendarModel:",
                string.Format( "Today {0} (day {1})", Today.ToString(dateFormat), Day ),
                "Started: " + StartDate.ToString(dateFormat),
            };

#if false && UNITY_EDITOR

            lines.Add( "Timeline Events: " + _timeline.Count.ToString() );
            foreach (var eventList in _timeline.OrderBy(kv => kv.Key).Select(kv => kv.Value))
            {
                foreach (var ev in eventList)
                {
                    lines.Add( string.Format( "  {0}: {1}", ev.Date.ToString(dateFormat), ev.ID ));
                }
            }
#endif
            return lines.ToArray();
        }

        public void Invoke( string[] args )
        {
            ConsoleModel.warn("CalendarModel has no invocation.");
        }    
    }
}
