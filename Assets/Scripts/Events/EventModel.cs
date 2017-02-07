using System;
using System.Collections.Generic;
using Core;
using Util;
using Newtonsoft.Json;

namespace DeWinter
{
	public class EventModel : DocumentModel
	{
		private const string DEFAULT_EVENT_TYPE = "party";

		private Dictionary<string, Dictionary<string, EventVO>> _events;

		[JsonProperty("events")]
		private Dictionary<string, Dictionary<string, EventVO>> _eventSetter
		{
			set
			{
				_events = value;
				foreach (KeyValuePair<string, Dictionary<string, EventVO>> typeList in _events)
				{
					foreach (KeyValuePair<string, EventVO> kvp in typeList)
					{
						_events[typeList.Key][kvp.Key].Type = typeList.Key;
						_events[typeList.Key][kvp.Key].EventID = kvp.Key;
					}
				}
			}
		}

		public EventModel() : base("EventData") {}

		public EventVO this[string eventID]
		{
			get
			{
				EventVO result;
				foreach (KeyValuePair<string, Dictionary<string, EventVO>> kvp in _events)
				{
					if (kvp.Value.TryGetValue(eventID, out result))
						return result;
				}
				return null;
			}
		}

		// Discard the current event and immediately returns the next event if one is avaliable.
		public EventVO CompleteEvent(EventVO e)
		{
			if (!_events.ContainsKey(e.Type)) return null;

			// If the plan is to keep a record of completed events, that would go here.
			_events[e.Type].Remove(e.EventID);
			if (!e.IsRandom || e.Options == null || e.Options.Count == 0) return null;
			int i=e.Options.Count;
			Random rnd = new Random();
			foreach (KeyValuePair<string, string> kvp in e.Options)
			{
				if (rnd.Next(i) == 0) return this[kvp.Key];
				i--;
			}
		}

		// Discard the current event and selects the specified event
		public EventVO CompleteEvent(EventVO e, string optionID)
		{
			if (!_events.ContainsKey(e.Type)) return null;

			// If the plan is to keep a record of completed events, that would go here.
			_events[e.Type].Remove(e.EventID);
			return this[optionID];
		}

		public EventVO GetRandomEvent(string type = DEFAULT_EVENT_TYPE)
		{
			Dictionary<string, EventVO> eventList;
			if (!_events.TryGetValue(type, out eventList)) return null;

			WeightedArray<EventVO> events = new WeightedArray<EventVO>();
			foreach (KeyValuePair<string, EventVO> kvp in eventList)
			{
				events.Add(kvp.Value, kvp.Value.Weight);
			}
			return events.Choose();
		}
	}
}