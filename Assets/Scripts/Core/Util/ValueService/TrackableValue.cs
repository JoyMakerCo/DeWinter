using System;
using System.Collections.Generic;

namespace Core
{
	public interface ITrackableValue {}

	public class TrackableValue<T> : ITrackableValue
	{
		protected T _baseValue;
		protected T _value;
		protected Dictionary<string, T> _modifiers = new Dictionary<string, T>();
		protected Dictionary<string, float> _multipliers = new Dictionary<string, float>();

		public T value
		{
			get { return _value; }
			set
			{
				_baseValue = value;
				foreach (KeyValuePair<string, float> kvp in _multipliers)
				{
					ApplyMultiplier(kvp.Key, kvp.Value);
				}
				foreach (KeyValuePair<string, T> kvp in _modifiers)
				{
					ApplyModifier(kvp.Key, kvp.Value);
				}
			}
		}

		protected virtual void ApplyMultiplier(string MultiplierID, float factor) {}
		protected virtual void ApplyModifier(string modifierID, T modifier) {}
	}
}
