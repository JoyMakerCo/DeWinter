using System;
using System.Collections.Generic;

namespace Core
{
// TODO: Implement C# Events instead of reinventing the wheel
	public class MessageSvc : IAppService, IDisposable
	{
		protected Dictionary<string, List<Delegate>> _listeners;
		protected Dictionary<Type, List<Delegate>> _typeListeners;
		protected List<KeyValuePair<string, Delegate>> _markedForRemoval;
		protected List<KeyValuePair<Type, Delegate>> _typeMarkedForRemoval;
		protected bool _isNotifying;

		public MessageSvc ()
		{
			_listeners = new Dictionary<string, List<Delegate>>();
			_typeListeners = new Dictionary<Type, List<Delegate>>();
			_markedForRemoval = null;
			_isNotifying = false;
		}

		public void Subscribe(string messageID, Action callback)
		{
			if (!_listeners.ContainsKey(messageID))
			{
				_listeners.Add(messageID, new List<Delegate>());
			}
			if (!_listeners[messageID].Contains(callback))
			{
				_listeners[messageID].Add(callback);
			}
		}

		public void Subscribe<T>(string messageID, Action<T> callback)
		{
			if (!_listeners.ContainsKey(messageID))
			{
				_listeners.Add(messageID, new List<Delegate>());
			}
			if (!_listeners[messageID].Contains(callback))
			{
				_listeners[messageID].Add(callback);
			}
		}

		public void Subscribe<T>(Action<T> callback)
		{
			Type t = typeof(T);
			if (!_typeListeners.ContainsKey(t))
			{
				_typeListeners.Add(t, new List<Delegate>());
			}
			if (!_typeListeners[t].Contains(callback))
			{
				_typeListeners[t].Add(callback);
			}
		}

		public void Unsubscribe(string messageID, Delegate callback)
		{
			if (!_isNotifying)
			{
				if (_listeners.ContainsKey(messageID))
				{
					_listeners[messageID].Remove(callback);
				}
			}
			else
			{
				if (_markedForRemoval == null)
				{
					_markedForRemoval = new List<KeyValuePair<string, Delegate>>();
				}
				_markedForRemoval.Add(new KeyValuePair<string, Delegate>(messageID, callback));
			}
		}

		public void Unsubscribe<T>(Action<T> callback)
		{
			Type t = typeof(T);
			if (!_isNotifying)
			{
				if (_typeListeners.ContainsKey(t))
				{
					_typeListeners[t].Remove(callback);
				}
			}
			else
			{
				if (_typeMarkedForRemoval == null)
				{
					_typeMarkedForRemoval = new List<KeyValuePair<Type, Delegate>>();
				}
				_typeMarkedForRemoval.Add(new KeyValuePair<Type, Delegate>(t, callback));
			}
		}
		
		public void Send(string messageID)
		{
			List<Delegate> callbacks;
			if (_listeners.TryGetValue(messageID, out callbacks))
			{
				_isNotifying = true;
				foreach(Delegate action in callbacks)
				{
					if (action is Action)
					{
						(action as Action).Invoke();
					}
				}
				RemoveMarkedDelegates();
			}
		}

		public void Send<T>(string messageID, T msg)
		{
			List<Delegate> callbacks;
			if (_listeners.TryGetValue(messageID, out callbacks))
			{
				_isNotifying = true;
				foreach(Delegate d in callbacks)
				{
					if (d is Action<T>)
						(d as Action<T>).Invoke(msg);
				}
				RemoveMarkedDelegates();
			}
		}

		public void Send<T>(T messageData)
		{
			List<Delegate> callbacks;
			Type t = typeof(T);
			if (_typeListeners.TryGetValue(t, out callbacks))
			{
				_isNotifying = true;
				foreach(Action<T> action in callbacks)
				{
					(action as Action<T>).Invoke(messageData);
				}
				RemoveMarkedDelegates();
			}
		}

		protected void RemoveMarkedDelegates()
		{
			_isNotifying = false;

			if (_markedForRemoval != null)
			{
				foreach(KeyValuePair<string, Delegate> kvp in _markedForRemoval)
				{
					if (_listeners.ContainsKey(kvp.Key))
					{
						_listeners[kvp.Key].Remove(kvp.Value);
					}
				}
				foreach(KeyValuePair<Type, Delegate> kvp in _typeMarkedForRemoval)
				{
					if (_typeListeners.ContainsKey(kvp.Key))
					{
						_typeListeners[kvp.Key].Remove(kvp.Value);
					}
				}
				_markedForRemoval.Clear();
				_markedForRemoval = null;
				_typeMarkedForRemoval.Clear();
				_typeMarkedForRemoval = null;
			}
		}

		public void Dispose()
		{
			RemoveMarkedDelegates();
			_listeners.Clear();
			_listeners = null;
			_typeListeners.Clear();
			_typeListeners = null;
		}
	}
}